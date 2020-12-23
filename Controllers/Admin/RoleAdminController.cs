using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using System.ComponentModel.DataAnnotations;
using AspShop.Models;
using System.Linq;
using Ext.Net.MVC;
using System.Collections.Generic;
using Ext.Net;
using AspShop.Helpers.App;
using System;
using AspShop.Helpers;
using AspShop.Models.View;
using AspShop.Helpers.Messages;
using AspShop.Helpers.Params;

namespace AspShop.Controllers.Admin
{
    [Authorize(Roles = "Administrators")]
    public class RoleAdminController : Controller
    {
        public FieldManager fieldManager = new FieldManager();
        public string addTo = "rightContainer";
        public ActionResult GetAll(StoreRequestParameters parameters)
        {
            try
            {
                int limit = parameters.Limit;//Лимит записей страницы.
                int start = parameters.Start;//Первая запись на странице.

                var items = RoleManager.Roles.ToList();

                if ((start + limit) > items.Count)
                {
                    limit = items.Count - start;
                }

                var rangeItems = (start < 0 || limit < 0) ? items : items.GetRange(start, limit);//Если start < 0 или limit < 0, то список получаемых ролей будет начинаться со start и заканчиваться limit.

                return this.Store(new Paging<AppRole>(rangeItems, items.Count));
            }
            catch
            {
                return this.Direct();
            }
        }
        public async Task<ActionResult> GetMembers(string id)
        {
            try
            {
                var role = await RoleManager.FindByIdAsync(id);
                string[] memberIDs = role.Users.Select(x => x.UserId).ToArray();
                var members = UserManager.Users.Where(x => memberIDs.Any(y => y == x.Id));

                return this.Store(members);
            }
            catch (Exception ex)
            {
                Err.Show(ex);
                return this.Direct();
            }
        }
        public async Task<ActionResult> GetNonMembers(string id)
        {
            try
            {
                var role = await RoleManager.FindByIdAsync(id);
                string[] memberIDs = role.Users.Select(x => x.UserId).ToArray();
                var members = UserManager.Users.Where(x => memberIDs.Any(y => y == x.Id));
                var nonMembers = UserManager.Users.Except(members);

                return this.Store(nonMembers);
            }
            catch (Exception ex)
            {
                Err.Show(ex);
                return this.Direct();
            } 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetById(string id)
        {
            try
            {
                var role = await RoleManager.FindByIdAsync(id);

                var panel = new Panel
                {
                    BodyPadding = 20,
                    Items =
                    {
                        new TextField
                        {
                            ID = "RoleId",
                            Width = 460,
                            FieldLabel = "Id роли",
                            AllowBlank = false,
                            BlankText = "Необходимо ввести Id роли",
                            Editable = false,
                            Text = role.Id
                        },
                        new TextField
                        {
                            ID = "RoleName",
                            Width = 460,
                            FieldLabel = "Название роли",
                            AllowBlank = false,
                            BlankText = "Необходимо ввести название роли",
                            Text = role.Name
                        },
                        new FieldSet
                        {
                            Title = "Пользователи в этой роли",
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
                                                    Url = Url.Action("GetMembers", "RoleAdmin"),
                                                    Reader =
                                                    {
                                                        new JsonReader
                                                        {
                                                            RootProperty = "data"
                                                        }
                                                    },
                                                    ExtraParams=
                                                    {
                                                        new Parameter("id", id, ParameterMode.Auto),
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
                                                DataIndex = "UserName",
                                                WidthSpec = "25%"
                                            },
                                            new Column
                                            {
                                                DataIndex = "Email",
                                                WidthSpec = "25%"
                                            },
                                            new Column
                                            {
                                                DataIndex = "RegDate",
                                                WidthSpec = "25%"
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
                        },
                        new FieldSet
                        {
                            Title = "Пользователи, не входящие в эту роль",
                            Items =
                            {
                                new GridPanel
                                {
                                    ID = "IdsToAdd",
                                    Scrollable = ScrollableOption.Disabled,
                                    Store =
                                    {
                                        new Store
                                        {
                                            Proxy =
                                            {
                                                new AjaxProxy
                                                {
                                                    Url = Url.Action("GetNonMembers", "RoleAdmin"),
                                                    Reader =
                                                    {
                                                        new JsonReader
                                                        {
                                                            RootProperty = "data"
                                                        }
                                                    },
                                                    ExtraParams=
                                                    {
                                                        new Parameter("id", id, ParameterMode.Auto),
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
                                                DataIndex = "UserName",
                                                WidthSpec = "25%"
                                            },
                                            new Column
                                            {
                                                DataIndex = "Email",
                                                WidthSpec = "25%"
                                            },
                                            new Column
                                            {
                                                DataIndex = "RegDate",
                                                WidthSpec = "25%"
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
                        },
                    },
                    Buttons =
                    {
                        new Button
                        {
                            Text = "Изменить",
                            FormBind = true,
                            DirectEvents =
                            {
                                Click =
                                {
                                    Url = Url.Action("Edit", "RoleAdmin"),
                                    Failure =
                                    @"Ext.MessageBox.show({
                                        title: 'Ошибка создания пользователя',
                                        msg: result.errorMessage,
                                        buttons: Ext.MessageBox.OK,
                                        icon: 'Error'
                                    });",
                                    ShowWarningOnFailure = false,
                                    ExtraParams=
                                    {
                                        new Parameter("RoleName", role.Name, ParameterMode.Auto),
                                        new Parameter("IdsToAdd", "function getNonMemb() { try { var j = []; JSON.parse(App.IdsToAdd.getSelectionSubmit().getSelectionModelField().value).forEach(function(element) { j.push(App.IdsToAdd.getStore().data.items[element.RowIndex].data.Id); }); } catch { } return j.join(); }", ParameterMode.Raw),
                                        new Parameter("IdsToDelete", "function getMemb() { try { var j = []; JSON.parse(App.IdsToDelete.getSelectionSubmit().getSelectionModelField().value).forEach(function(element) { j.push(App.IdsToDelete.getStore().data.items[element.RowIndex].data.Id); }); } catch { } return j.join(); }", ParameterMode.Raw),
                                        new Parameter("__RequestVerificationToken", AntiForgeryField.GetField())
                                    }
                                }
                            }
                        }
                    }
                };

                this.GetCmp<Panel>(addTo).RemoveAll();
                panel.AddTo(this.GetCmp<Panel>(addTo));//Добавляем панель с сгенерированными полями на в панель-контейнер в представлении.

                return this.Direct();
            }
            catch (Exception ex)
            {
                Err.Show(ex);
                return this.Direct();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoleEditModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    X.Msg.Alert("Ошибка редактирования роли", "Введены ошибочные данные").Show();
                    return this.Direct();
                }

                var IdsToAdd = new List<string>();
                var IdsToDelete = new List<string>();

                if (model.IdsToAdd != null)
                {
                    IdsToAdd.AddRange(model.IdsToAdd.Split(','));
                }
                if (model.IdsToDelete != null)
                {
                    IdsToDelete.AddRange(model.IdsToDelete.Split(','));
                }

                foreach (string userId in IdsToAdd.ToArray() ?? new string[] { })
                {
                    var result = await UserManager.AddToRoleAsync(userId, model.RoleName);

                    if (!result.Succeeded)
                    {
                        X.Msg.Alert("Ошибка", string.Join(Environment.NewLine, result.Errors)).Show();
                        return this.Direct();
                    }
                }
                foreach (string userId in IdsToDelete.ToArray() ?? new string[] { })
                {
                    var result = await UserManager.RemoveFromRoleAsync(userId,
                    model.RoleName);

                    if (!result.Succeeded)
                    {
                        X.Msg.Alert("Ошибка", string.Join(Environment.NewLine, result.Errors)).Show();
                        return this.Direct();
                    }
                }

                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex)
            {
                Err.Show(ex);
                return this.Direct();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InitCreate(string rootEvent, string addTo)
        {
            try
            {
                var editPanel = new Panel
                {
                    BodyPadding = 20,
                    Items =
                    {
                        new FieldSet
                        {
                            Title = "Пользователь",
                            Items =
                            {
                                new TextField
                                {
                                    ID = "RoleName",
                                    Width = 460,
                                    FieldLabel = "Название роли",
                                    AllowBlank = false,
                                    BlankText = "Необходимо ввести название роли",
                                },
                            }
                        }
                    },
                    Buttons =
                    {
                        new Button
                        {
                            Text = "Создать новую роль",
                            DirectEvents =
                            {
                                Click =
                                {
                                    Url = Url.Action("Create", "RoleAdmin"),
                                    ShowWarningOnFailure = false,
                                    ExtraParams =
                                    {
                                        new Parameter("roleName", "App.RoleName.value", ParameterMode.Raw),
                                        new Parameter("__RequestVerificationToken", AntiForgeryField.GetField()),
                                    }
                                }
                            }
                        }
                    }
                };

                this.GetCmp<Store>("UserStore").Reload();
                this.GetCmp<Panel>(addTo).RemoveAll();
            }          
            catch (Exception ex)
            {
                Err.Show(ex);
            }

            return this.Direct();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Required]string roleName)
        {
            if (ModelState.IsValid)
            {
                var result = await RoleManager.CreateAsync(new AppRole(roleName));

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {

                }
            }
            return this.Direct();
        }
        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
        private AppRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();
            }
        }
    }
}