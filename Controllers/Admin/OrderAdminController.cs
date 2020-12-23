using AspShop.Helpers;
using AspShop.Helpers.App;
using AspShop.Models.Items;
using Ext.Net;
using Ext.Net.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AspShop.Helpers.Params;
using AspShop.Helpers.Messages;
using AspShop.Models.View;

namespace AspShop.Controllers.Admin
{
    public class OrderAdminController : Controller
    {
        public AppDbContext db = new AppDbContext();
        public FieldManager fieldManager = new FieldManager();
        public string addTo = "rightContainer";
        public ActionResult GetAll(StoreRequestParameters parameters)
        {
            int limit = parameters.Limit;
            int start = parameters.Start;

            var items = db.Orders.Where(x => x.OrderDate != null).ToList();

            if ((start + limit) > items.Count)
            {
                limit = items.Count - start;
            }

            var rangeItems = (start < 0 || limit < 0) ? items : items.GetRange(start, limit);

            return this.Store(new Paging<Order>(rangeItems, items.Count));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetById(Guid id)
        {
            try
            {
                var order = db.Orders.First(x => x.Id == id);

                var panel = fieldManager.RenderFields(new FieldCreateParams
                {
                    ModelsForParse = new List<object> { order },//orderItems
                    ButtonUrlAction = Url.Action("Edit", "OrderAdmin"),
                    ButtonText = "Редактировать заказ"
                });

                var gridpanel = new FieldSet
                {
                    Title = "Элементы заказа",
                    Items =
                    {
                        new GridPanel
                        {
                            ID = "IdsToDelete",
                            Scrollable = ScrollableOption.Disabled,
                            Store =
                            {
                                new Store
                                {
                                    Proxy =
                                    {
                                        new AjaxProxy
                                        {
                                            Url = Url.Action("GetItems", "OrderAdmin"),
                                            Reader =
                                            {
                                                new JsonReader
                                                {
                                                    RootProperty = "data"
                                                }
                                            },
                                            ExtraParams=
                                            {
                                                new Parameter("id", order.Id.ToString(), ParameterMode.Auto),
                                            },
                                        },
                                    }
                                }
                            },
                            ColumnModel =
                            {
                                Columns =
                                {
                                    new Column
                                    {
                                        DataIndex = "Id",
                                        Hidden = true,
                                    },
                                    new TemplateColumn
                                    {
                                        Width = 60,
                                        Tpl = new XTemplate
                                        {
                                            Html = @"<img class='search-image' src='{Image}'/>"
                                        }
                                    },
                                    new Column
                                    {
                                        DataIndex = "Name",
                                        WidthSpec = "35%",
                                    },
                                    new Column
                                    {
                                        DataIndex = "ItemsCount",
                                        WidthSpec = "35%",
                                    },
                                }
                            },
                            SelectionModel =
                            {
                                new CheckboxSelectionModel
                                {
                                    Mode = SelectionMode.Multi
                                }
                            }
                        },
                    }
                };

                panel.Items.Add(gridpanel);

                this.GetCmp<Panel>(addTo).RemoveAll();

                panel.AddTo(this.GetCmp<Panel>(addTo));//Добавляем панель с сгенерированными полями на в панель-контейнер в представлении.
            }
            catch (Exception ex)
            {
                Err.Show(ex);
            }

            return this.Direct();
        }

        public ActionResult GetItems(Guid id)
        {
            try
            {
                var obj = new List<OrderViewModel>();
                var order = db.Orders.First(x => x.Id == id);

                var orderItems = db.OrderItems.Where(x => x.OrderId == order.Id);

                foreach (var orderItem in orderItems)
                {
                    var item = db.Items.FirstOrDefault(x => x.Id == orderItem.ItemId);
                    obj.Add(new OrderViewModel
                    {
                        Id = item.Id,
                        Image = item.Image,
                        Name = item.Name,
                        ItemsCount = orderItem.ItemsCount
                    });
                }

                return this.Store(obj);
            }
            catch (Exception ex)
            {
                Err.Show(ex);
                return this.Direct();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InitCreate()
        {
            try
            {
                var panel = fieldManager.RenderFields(new FieldCreateParams
                {
                    ModelsForParse = new List<object> { new Order() },
                    ButtonUrlAction = Url.Action("Create", "UserAdmin"),
                    ButtonText = "Создать новый заказ",
                    IsForCreate = true
                });

                panel.AddTo(this.GetCmp<Panel>(addTo));

                return this.Direct();
            }
            catch (Exception ex)
            {
                Err.Show(ex, true);
                return this.Direct();
            }
        }
      /*  [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection formcol)
        {
            var list = fieldManager.EditModel(new FieldEditParams
            {
                ModelsForEdit = new List<Type> { typeof(Item), typeof(Laptop), typeof(Smartphone) },
                InputCollection = formcol
            });

            return RedirectToAction("Index", "Admin");
        }*/
    }
}