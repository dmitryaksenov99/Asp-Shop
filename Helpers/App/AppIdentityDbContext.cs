using AspShop.Models;
using AspShop.Models.Items;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AspShop.Helpers.App
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Item> Items { get; set; }//Контейнер товара, включающий в себя общие свойства, характерные для любого из существующих товаров.
        public DbSet<Laptop> Laptops { get; set; }//Сам товар (для примера).
        public DbSet<Smartphone> Smartphones { get; set; }//Ещё один товар.
        public DbSet<Order> Orders { get; set; }//Заказы пользователей.
        public DbSet<OrderItem> OrderItems { get; set; }//Элементы заказов пользователей.
        public AppDbContext() : base("DefaultConnection", throwIfV1Schema: false) { }
        public static AppDbContext Create()
        {
            return new AppDbContext();
        }
    }
}