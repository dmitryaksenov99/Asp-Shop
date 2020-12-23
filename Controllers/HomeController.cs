using AspShop.Helpers;
using AspShop.Helpers.App;
using AspShop.Helpers.Generators;
using AspShop.Models;
using Ext.Net;
using Ext.Net.MVC;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;

namespace AspShop.Controllers
{
    public class HomeController : Controller
    {
        public AppDbContext db = new AppDbContext();
        public ActionResult Index()
        {
            ViewBag.Viewport = Layout.GetViewport();

    /*        db.Items.Add(GoodGenerator.Laptop());
              db.Items.Add(GoodGenerator.Smartphone());
              db.SaveChanges();*/

            /*ExtNetModel model = new ExtNetModel()
             {
                 Title = "Welcome to Ext.NET",
                 TextAreaEmptyText = ">> Enter a Message Here <<"
             };*/

            return this.View(db.Items);
        }

        private AppUser CurrentUser
        {
            get
            {
                return UserManager.FindById(User.Identity.GetUserId());
            }
        }

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
        public ActionResult SampleAction(string message)
        {
            X.Msg.Notify(new NotificationConfig
            {
                Icon = Icon.Accept,
                Title = "Working",
                Html = message
            }).Show();

            return this.Direct();
        }
    }
}