using AspShop.Helpers;
using AspShop.Helpers.App;
using AspShop.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspShop.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class AdminController : Controller
    {
        public AppDbContext db = new AppDbContext();
        public ActionResult Index()
        {
            ViewBag.Viewport = Layout.GetViewport();

            return this.View();
        }
    }
}