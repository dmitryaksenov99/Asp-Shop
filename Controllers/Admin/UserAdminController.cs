using AspShop.Helpers;
using AspShop.Helpers.App;
using AspShop.Helpers.Generators;
using AspShop.Helpers.Messages;
using AspShop.Helpers.Params;
using AspShop.Models;
using Ext.Net;
using Ext.Net.MVC;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AspShop.Controllers.Admin
{
    [Authorize(Roles = "Administrators")]
    public class UserAdminController : Controller
    {
        public FieldManager fieldManager = new FieldManager();
        public string addTo = "rightContainer";
        private AppUser CurrentUser
        {
            get
            {
                return UserManager.FindById(User.Identity.GetUserId());
            }
        }

        public ActionResult GetAll(StoreRequestParameters parameters)//Получаем всех пользователей со включённой пагинацией.
        {
                 /*  var user = CurrentUser;
                   user.Customer = new Customer
                   {
                       Code = RandomStringGenerator.GenerateCustomerCode(),
                       Name = "ADMIN",
                       Discount = 0,
                       Address = "ADDRESS"
                   };
                   var result = UserManager.Update(user);*/
                   
            try
            {
                int limit = parameters.Limit;//Лимит записей страницы.
                int start = parameters.Start;//Первая запись на странице.

                var items = UserManager.Users.ToList();

                if ((start + limit) > items.Count)
                {
                    limit = items.Count - start;
                }

                var rangeItems = (start < 0 || limit < 0) ? items : items.GetRange(start, limit);//Если start < 0 или limit < 0, то список получаемых пользователей будет начинаться со start и заканчиваться limit.

                return this.Store(new Paging<AppUser>(rangeItems, items.Count));
            }
            catch
            {
                return this.Direct();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetById(string id)//Получаем пользователя по id.
        {
            try
            {
                var user = await UserManager.FindByIdAsync(id);//Поиск пользователя по id.
                var panel = fieldManager.RenderFields(new FieldCreateParams//Вызываем метод класса отрисовки полей.
                {
                    ModelsForParse = new List<object> { user, user.Customer },//Передаём объекты с данными, исходя из содержимого которых нужно отрисовать поля.
                    ButtonUrlAction = Url.Action("Edit", "UserAdmin"),//Назначаем действие для кнопки, которая будет находиться внизу панели с данными.
                    ButtonText = "Редактировать пользователя"//Текст для кнопки.
                });

                this.GetCmp<Panel>(addTo).RemoveAll();
                panel.AddTo(this.GetCmp<Panel>(addTo));//Добавляем панель с сгенерированными полями на в панель-контейнер в представлении.

                return this.Direct();
            }
            catch (Exception ex)
            {
                Err.Show(ex, true);
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
                    ModelsForParse = new List<object> { new AppUser(), new Customer() },
                    ButtonUrlAction = Url.Action("Create", "UserAdmin"),
                    ButtonText = "Создать нового пользователя",
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FormCollection formcol)
        {
            try
            {
                var modelsForCreate = new List<Type> { typeof(AppUser), typeof(Customer) };//Список типов моделей, на основе которых будет производиться обработка входных данных.
                var modelNames = formcol.AllKeys.ToList();//Приводим все ключи коллекции из параметров с данными отрендеренных полей к списку. Ключи это названия FieldSet-ов.
                modelNames.Remove("__RequestVerificationToken");//Удаляем из списка ключей AntiForgery-токен, потому что он не является названием класса ни одной из существующих моделей.

                AppUser user = new AppUser//Создаём пустой экземпляр класса пользователя, содержащий в себе лишь пустой класс покупателя.
                {
                    Customer = new Customer()//Пустой класс покупателя.
                };

                string modelPassword = null;//Создаём временную переменную для пароля пользователя.

                foreach (var modelName in modelNames)//Обрабатываем список названий моделей.
                {
                    var currentType = modelsForCreate.First(type => type.Name.Split('_')[0] == modelName);//Получаем тип текущей модели.
                    string rawJson = formcol[modelName];//Получаем json-value с данными сгенерированных полей, соответствующее ключу formcol.

                    if (modelName == nameof(AppUser))//Если название текущей модели соответствует названию модели пользователя, начинаем десериализацию raw-данных в модель.
                    {
                        var toCreate = (AppUser)JsonConvert.DeserializeObject(rawJson, currentType);

                        user.UserName = toCreate.UserName;
                        user.Email = toCreate.Email;
                        user.PhoneNumber = toCreate.PhoneNumber;
                        user.RegDate = DateTime.Now;

                        var validateUser = await UserManager.UserValidator.ValidateAsync(user);
                        if (validateUser.Succeeded == false)
                        {
                            Msg.Show(string.Join(Environment.NewLine, validateUser.Errors), "Ошибка валидации");
                            return this.Direct();
                        }

                        var validatePassword = await UserManager.PasswordValidator.ValidateAsync(toCreate.ModelPassword);
                        if (validatePassword.Succeeded == false)
                        {
                            Msg.Show(string.Join(Environment.NewLine, validatePassword.Errors), "Ошибка валидации");
                            return this.Direct();
                        }
                        else
                        {
                            modelPassword = toCreate.ModelPassword;
                        }
                    }
                    else if (modelName == nameof(Customer))//Если название текущей модели соответствует названию модели покупателя, начинаем десериализацию raw-данных в модель.
                    {
                        var toCreate = (Customer)JsonConvert.DeserializeObject(rawJson, currentType);//Десериализуем rawJson в покупателя.

                        if (!string.IsNullOrWhiteSpace(toCreate.Name))//Проверяем, не является ли поле пустым.
                        {
                            user.Customer.Name = toCreate.Name;//Обновляем поля покупателя.
                        }
                        if (!string.IsNullOrWhiteSpace(toCreate.Address))
                        {
                            user.Customer.Address = toCreate.Address;
                        }

                        user.Customer.Code = RandomStringGenerator.GenerateCustomerCode();

                        if (toCreate.Discount! >= 0)
                        {
                            user.Customer.Discount = toCreate.Discount;
                        }
                        else
                        {
                            user.Customer.Discount = 0;
                        }
                    }
                    else
                    {
                        Msg.Show("Неизвестная входная модель", "Ошибка");
                        return this.Direct();
                    }
                }

                var result = await UserManager.CreateAsync(user, modelPassword);//Пытаемся создать пользователя.

                if (result.Succeeded)//Если пользователь создался.
                {
                    this.GetCmp<Store>("UserStore").Reload();//Перезагружаем список пользователей.
                    this.GetCmp<Panel>(addTo).RemoveAll();//Очищаем содержимое области с полями пользователя.
                    return this.Direct();
                }
                else
                {
                    Msg.Show(string.Join(Environment.NewLine, result.Errors), "Ошибка создания пользователя");
                    return this.Direct();
                }
            }
            catch (JsonReaderException ex)
            {
                Err.Show(ex);
                return this.Direct();
            }
            catch (JsonSerializationException ex)
            {
                Err.Show(ex);
                return this.Direct();
            }
            catch (Exception ex)
            {
                Err.Show(ex, true);
                return this.Direct();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(FormCollection formcol)//Редактируем пользователя. Formcol - коллекция из названий FieldSet-ов, которые являются группами для полей из разных моделей, и значений - json с данными этих полей.
        {
            try
            {
                var modelsForEdit = new List<Type> { typeof(AppUser), typeof(Customer) };//Типы моделей, которые мы будем редактировать.
                var modelNames = formcol.AllKeys.ToList();//Приводим все ключи коллекции из параметров с данными отрендеренных полей к списку. Ключи это названия FieldSet-ов.
                modelNames.Remove("__RequestVerificationToken");//Удаляем из списка ключей AntiForgery-токен, потому что он не является названием класса ни одной из существующих моделей.

                AppUser user = null;//Создаём пустого пользователя.
                string modelPassword = null;//Создаём временную переменную для пароля пользователя.

                foreach (var modelName in modelNames)//Обрабатываем список названий моделей.
                {
                    var currentType = modelsForEdit.First(type => type.Name.Split('_')[0] == modelName);//Получаем тип текущей модели.
                    string rawJson = formcol[modelName];//Получаем json-value с данными сгенерированных полей, соответствующее ключу formcol.

                    if (modelName == nameof(AppUser))//Если название текущей модели соответствует названию модели пользователя, начинаем десериализацию raw-данных в модель.
                    {
                        var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd.MM.yyyy HH:mm:ss" };
                        var toEdit = (AppUser)JsonConvert.DeserializeObject(rawJson, currentType, dateTimeConverter);//Десериализуем rawJson в пользователя, с помощью данных которого мы будем редактировать пользователя из БД с соответствующим этому пользователю Id
                        user = await UserManager.FindByIdAsync(toEdit.Id);//Находим пользователя по Id

                        user.UserName = toEdit.UserName;//Обновляем имя найденного пользователя.
                        user.Email = toEdit.Email;//Обновляем почту найденного пользователя.
                        user.PhoneNumber = toEdit.PhoneNumber;//Обновляем номер телефона найденного пользователя.
                        user.RegDate = toEdit.RegDate;//Обновляем дату регистрации.
                        modelPassword = toEdit.ModelPassword;

                        var validateUser = await UserManager.UserValidator.ValidateAsync(user);
                        if (validateUser.Succeeded == false)
                        {
                            Msg.Show(string.Join(Environment.NewLine, validateUser.Errors), "Ошибка валидации");
                            return this.Direct();
                        }
                    }
                    else if (modelName == nameof(Customer))//Если название текущей модели соответствует названию модели покупателя, начинаем десериализацию raw-данных в модель.
                    {
                        var toModify = (Customer)JsonConvert.DeserializeObject(rawJson, currentType);//Десериализуем rawJson в покупателя.

                        if (!string.IsNullOrWhiteSpace(toModify.Name))//Проверяем, не является ли поле пустым.
                        {
                            user.Customer.Name = toModify.Name;//Обновляем поля покупателя.
                        }
                        if (!string.IsNullOrWhiteSpace(toModify.Address))
                        {
                            user.Customer.Address = toModify.Address;
                        }
                        if (!string.IsNullOrWhiteSpace(toModify.Code))
                        {
                            user.Customer.Code = toModify.Code;
                        }
                        if (toModify.Discount! >= 0)
                        {
                            user.Customer.Discount = toModify.Discount;
                        }
                        else
                        {
                            user.Customer.Discount = 0;
                        }
                    }
                    else
                    {
                        Msg.Show("Неизвестная входная модель", "Ошибка");//Если модель, которая находится в formcol не является ни одной из описанных в цикле моделей, выводим сообщение об ошибке.
                        return this.Direct();
                    }
                }

                if (!string.IsNullOrWhiteSpace(modelPassword))//Проверяем, не является ли пароль пустым.
                {
                    var validPass = await UserManager.PasswordValidator.ValidateAsync(modelPassword);

                    if (validPass.Succeeded)//Если новый пароль соответствует критериям пароля, заменяем им старый пароль без подтверждения старого пароля.
                    {
                        user.PasswordHash = UserManager.PasswordHasher.HashPassword(modelPassword);

                    }
                    else
                    {
                        Msg.Show(string.Join(Environment.NewLine, validPass.Errors), "Ошибка обновления пароля");
                    }
                }
                else
                {
                    //В случае, если поле пароля пустое, оставляем старый пароль.
                }

                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    this.GetCmp<Store>("UserStore").Reload();
                    this.GetCmp<Panel>(addTo).RemoveAll();
                    return this.Direct();
                }
                else
                {
                    Msg.Show(string.Join(Environment.NewLine, result.Errors), "Ошибка изменения пользователя");
                    return this.Direct();
                }
            }
            catch (Exception ex)
            {
                Err.Show(ex, true);
                return this.Direct();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var user = await UserManager.FindByIdAsync(id);//Находим пользователя по id
                if (user == null)
                {
                    Msg.Show("Пользователь не найден");
                    return this.Direct();
                }
                if (UserManager.IsInRole(user.Id, "Administrators"))
                {
                    Msg.Show("Удаление администратора невозможно");
                    return this.Direct();
                }

                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    Msg.Show(string.Join(Environment.NewLine, result.Errors));
                }
            }
            catch (Exception ex)
            {
                Err.Show(ex, true);
            }

            return this.Direct();
        }
        public AppSignInManager SignInManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<AppSignInManager>();
            }
        }
        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
    }
}