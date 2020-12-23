using AspShop.Helpers.App;
using AspShop.Models.View;
using Ext.Net;
using Ext.Net.MVC;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AspShop.Controllers.Account
{
    public class AuthController : Controller
    {
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                X.Msg.Alert("Ошибка авторизации", "Введены ошибочные данные").Show();
                return this.Direct();
            }

            var result = await SignInManager.PasswordSignInAsync(model.EmailOrNumber, model.Password, model.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    var user = new ClaimsPrincipal(SignInManager.AuthenticationManager.AuthenticationResponseGrant.Identity);
                    if (user.IsInRole("Administrators"))
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                case SignInStatus.LockedOut:
                    X.Msg.Alert("Ошибка авторизации", "Пользователь заблокирован").Show();
                    return this.Direct();
                case SignInStatus.Failure:
                    X.Msg.Alert("Ошибка авторизации", "Неправильные логин или пароль").Show();
                    return this.Direct();
                default:
                    X.Msg.Alert("Ошибка авторизации", "Неудачная попытка входа.").Show();
                    return this.Direct();
            }
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            SignInManager.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
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