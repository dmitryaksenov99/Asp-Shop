using AspShop.Helpers;
using AspShop.Helpers.App;
using AspShop.Helpers.Generators;
using AspShop.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity.Migrations;

namespace AspShop.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "AspShop.Infrastructure.AppIdentityDbContext";
        }

        protected override void Seed(AppDbContext db)
        {
            var userMgr = new AppUserManager(new UserStore<AppUser>(db));
            var roleMgr = new AppRoleManager(new RoleStore<AppRole>(db));

            string roleName = "Administrators";
            string userName = "admin";
            string password = "qwerty";
            string email = "admin@gmail.com";

            if (!roleMgr.RoleExists(roleName))
            {
                roleMgr.Create(new AppRole(roleName));
            }

            var user = userMgr.FindByName(userName);

            if (user == null)
            {
                userMgr.Create(new AppUser
                {
                    UserName = userName,
                    Email = email,
                    RegDate = DateTime.Now
                }, password);

                user = userMgr.FindByName(userName);
            }

            for (int i = 0; i < 10; i++)
            {
                db.Items.Add(GoodGenerator.Laptop());
                db.Items.Add(GoodGenerator.Smartphone());
                db.SaveChanges();
            }

            if (!userMgr.IsInRole(user.Id, roleName))
            {
                userMgr.AddToRole(user.Id, roleName);
            }

            db.SaveChanges();
        }
    }
}
