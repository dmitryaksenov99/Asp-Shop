using AspShop.Helpers.App;
using AspShop.Helpers.Generators;
using AspShop.Helpers.Messages;
using AspShop.Helpers.Params;
using AspShop.Models;
using AspShop.Models.Enums;
using AspShop.Models.Items;
using Ext.Net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AspShop.Helpers
{
    public class ParsedField
    {
        public string FieldName { get; set; }
        public object FieldValue { get; set; }
        public Dictionary<string, object> Attributes { get; set; }
    }
    public class FieldManager
    {
        public AppDbContext db = new AppDbContext();
        public Panel RenderFields(FieldCreateParams @params)
        {
            var editPanel = new Panel
            {
                BodyPadding = @params.PanelBodyPadding,
            };

            try
            {
                var button = new Button
                {
                    Text = @params.ButtonText,
                    DirectEvents =
                    {
                        Click =
                        {
                            Url = @params.ButtonUrlAction,
                            ShowWarningOnFailure = true,
                        }
                    }
                };

                var parsedModels = ParseModelsFields(@params.ModelsForParse);

                foreach (var model in parsedModels)
                {
                    var fieldSet = new FieldSet
                    {
                        Title = model.Key,//название таблицы
                        ID = model.Key//название таблицы
                    };

                    foreach (var field in model.Value)
                    {
                        try
                        {
                            dynamic fieldLabel = "";
                            if (field.Attributes.TryGetValue(nameof(DescriptionAttribute), out fieldLabel) == false)
                            {
                                continue;
                            }

                            string fieldText = "";
                            bool fieldEditable = true;
                            bool disallowedFields =
                                field.FieldName == "Id" ||
                                field.FieldName == "Code" ||
                                field.FieldName == nameof(AppUser.RegDate);
                            if (@params.IsForCreate)
                            {
                                if (disallowedFields)
                                {
                                    continue;
                                }
                                if (field.FieldName == nameof(Customer.Discount))
                                {
                                    fieldText = "0";
                                }
                            }
                            else
                            {
                                if (disallowedFields)
                                {
                                    fieldEditable = false;
                                }
                                if (field.FieldName == nameof(AppUser.ModelPassword))
                                {
                                    fieldText = "";
                                }
                                else
                                {
                                    fieldText = field.FieldValue.ToString();
                                } 
                            }

                            string fieldId = RandomStringGenerator.GenerateFieldId(field.FieldName, 6);
                            dynamic fieldComponent = null;

                            if (field.FieldName == nameof(Item.Description) || field.FieldName == nameof(Laptop.AdditionalFatures))
                            {
                                fieldComponent = new TextArea
                                {
                                    ID = fieldId,
                                    Height = @params.DescriptionHeight,
                                    WidthSpec = @params.WidthSpec,
                                    FieldLabel = fieldLabel,
                                    Text = fieldText
                                };
                            }
                            else if (field.FieldName == nameof(Laptop.RamType))
                            {
                                fieldComponent = new ComboBox
                                {
                                    ID = fieldId,
                                    WidthSpec = @params.WidthSpec,
                                    FieldLabel = fieldLabel,
                                    Text = fieldText,
                                    Items =
                                    {
                                        new ListItem(Enum.GetName(typeof(RamType), RamType.DDR3)),
                                        new ListItem(Enum.GetName(typeof(RamType), RamType.DDR4)),
                                    }
                                };
                            }
                            else if (field.FieldName == nameof(Smartphone.DisplayType))
                            {
                                fieldComponent = new ComboBox
                                {
                                    ID = fieldId,
                                    WidthSpec = @params.WidthSpec,
                                    FieldLabel = fieldLabel,
                                    Text = fieldText,
                                    Items =
                                    {
                                        new ListItem(Enum.GetName(typeof(DisplayType), DisplayType.IPS)),
                                        new ListItem(Enum.GetName(typeof(DisplayType), DisplayType.TN)),
                                        new ListItem(Enum.GetName(typeof(DisplayType), DisplayType.VA)),
                                    }
                                };
                            }
                            else
                            {
                                fieldComponent = new TextField
                                {
                                    ID = fieldId,
                                    WidthSpec = @params.WidthSpec,
                                    FieldLabel = fieldLabel,
                                    Editable = fieldEditable,
                                    Text = fieldText
                                };
                            }

                            fieldSet.Items.Add(fieldComponent);
                        }
                        catch (NullReferenceException ex)
                        {

                        }
                        catch (Exception ex)
                        {
                            Err.Show(ex, true);
                        }
                    }

                    button.DirectEvents.Click.ExtraParams.Add(new Parameter(model.Key, "function getFields() { var json = {}; var obj = App." + model.Key + ".items.getRange(); obj.map(a=> a.id.slice(0, -6)).forEach(function (k, v) { json[k] = obj.map(a=> a.rawValue)[v]; }); return json; }", ParameterMode.Raw));
                    editPanel.Items.Add(fieldSet);
                }

                button.DirectEvents.Click.ExtraParams.Add(new Parameter("__RequestVerificationToken", AntiForgeryField.GetField()));
                editPanel.Buttons.Add(button);
            }
            catch (Exception ex)
            {
                Err.Show(ex, true);
            }

            return editPanel;
        }

        public bool EditModel(FieldEditParams @params)
        {
            try
            {
                var classNames = @params.InputCollection.AllKeys.ToList();
                classNames.Remove("__RequestVerificationToken");

                foreach (var className in classNames)
                {
                    var currentType = @params.ModelsForEdit.First(type=> type.Name.Split('_')[0] == className);
                    string rawJson = @params.InputCollection[className];
                    object toModify = JsonConvert.DeserializeObject(rawJson, currentType);
                    db.Entry(toModify).State = EntityState.Modified;
                }

                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Dictionary<string, List<ParsedField>> ParseModelsFields(List<object> models)
        {
            var parsedModels = new Dictionary<string, List<ParsedField>>();

            foreach (var model in models.Where(model => model != null))
            {
                var type = model.GetType();

                var fieldNames = type.GetProperties().Select(field => field.Name).ToList();//названия всех полей модели
                var fieldValues = type.GetProperties().Select(field => field.GetValue(model)).ToList();//значения всех полей модели
                var fieldValNames = fieldNames.Zip(fieldValues, (key, value) => new { key, value }).ToDictionary(x => x.key, x => x.value);//создание словаря из имён и значений полей

                var parsedFields = new List<ParsedField>();

                foreach (var item in fieldValNames)
                {
                    try
                    {
                        var attNames = type.GetProperty(item.Key).GetCustomAttributes().Select(n => n.GetType().Name).ToList();//названия аттрибутов текущего поля модели
                        var attValues = type.GetProperty(item.Key).GetCustomAttributesData().SelectMany(c => c.ConstructorArguments.Select(v => v.Value)).ToList();//содержимое аттрибутов текущего поля модели
                        var attValNames = attNames.Zip(attValues, (key, value) => new { key, value }).ToDictionary(x => x.key, x => x.value);//создание словаря из имён имён и значений атрибутов

                        parsedFields.Add(new ParsedField
                        {
                            FieldName = item.Key,
                            FieldValue = item.Value,
                            Attributes = attValNames
                        });
                    }
                    catch (Exception ex)
                    {

                    }
                }

                parsedModels.Add(type.Name.Split('_')[0], parsedFields);
            }

            return parsedModels;
        }
    }
}