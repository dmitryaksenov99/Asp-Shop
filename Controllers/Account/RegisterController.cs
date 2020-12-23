using AspShop.Helpers;
using AspShop.Helpers.App;
using AspShop.Helpers.Generators;
using AspShop.Models;
using AspShop.Models.View;
using Ext.Net;
using Ext.Net.MVC;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AspShop.Controllers
{
    public class RegisterController:Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Viewport = Layout.GetViewport();
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                X.Msg.Alert("Ошибка регистрации", "Введены ошибочные данные").Show();
                return this.Direct();
            }

            string userName = "";

            if (model.Email != null)
            {
                userName = model.Email;
            }
            else if(model.PhoneNumber != null)
            {
                userName = model.PhoneNumber;
            }
            else
            {
                userName = "NULL";
            }

            try
            {
                var user = new AppUser
                {
                    UserName = userName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    RegDate = DateTime.Now,
                    Customer = new Customer
                    {
                        Code = RandomStringGenerator.GenerateCustomerCode(),
                        Name = model.Name
                    }
                };
              //  user.Customer.Code = RandomStringGenerator.GenerateCustomerCode();
           ////     user.Customer.Name = model.Name;

                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: model.RememberMe, rememberBrowser: false);
                    return RedirectToAction("Index", "Home");  
                }
                else
                {
                    X.Msg.Alert("Ошибка регистрации", string.Join(Environment.NewLine, result.Errors)).Show();
                }
            }
            catch (Exception ex)
            {
                X.Msg.Alert("Ошибка регистрации", ex.Message.ToString()).Show();   
            }

            return this.Direct();
        }
        public AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
        public AppSignInManager SignInManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<AppSignInManager>();
            }
        }
    }
}