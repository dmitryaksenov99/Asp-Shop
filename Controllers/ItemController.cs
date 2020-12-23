using AspShop.Helpers;
using AspShop.Helpers.App;
using AspShop.Models.Enums;
using AspShop.Models.Items;
using Ext.Net.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace AspShop.Controllers
{
    public class ItemController : Controller
    {
        public AppDbContext db = new AppDbContext();
        public ActionResult Index(Guid id)
        {
            ViewBag.Viewport = Layout.GetViewport();
            var item = db.Items.Find(id);

            ViewBag.Item = item;

            if (item.Category == Category.Laptop.ToString())
            {
                var obj = new List<object[]>
                {
                    new object[2] { "Процессор",
                        item.Laptop.Cpu },
                    new object[2] { "Диагональ экрана",
                        item.Laptop.ScreenDiagonal },
                    new object[2] { "Объем оперативной памяти",
                        item.Laptop.RamSize },
                    new object[2] { "Тип оперативной памяти",
                        item.Laptop.RamType },
                    new object[2] { "Операционная система",
                        item.Laptop.Os },
                    new object[2] { "Накопитель",
                        item.Laptop.Storage },
                    new object[2] { "Дополнительные возможности",
                        item.Laptop.AdditionalFatures },
                    new object[2] { "Графический адаптер",
                        item.Laptop.GraphicsAdapter },
                    new object[2] { "Разъемы и порты",
                        item.Laptop.Connectors },
                    new object[2] { "Аккумулятор",
                        item.Laptop.Battery },
                    new object[2] { "Габариты ",
                        item.Laptop.Dimensions },
                    new object[2] { "Вес",
                        item.Laptop.Weight }
                };

                return this.View(obj);
            }
            else if (item.Category == Category.Smartphone.ToString())
            {
                var obj = new List<object[]>
                {
                    new object[2] { "Производитель",
                        item.Brand },
                    new object[2] { "Диагональ экрана",
                        item.Smartphone.DisplayDiagonal },
                    new object[2] { "Тип экрана",
                        item.Smartphone.DisplayType },
                    new object[2] { "Объём ОЗУ",
                        item.Smartphone.RamSize },
                    new object[2] { "Объём памяти",
                        item.Smartphone.Storage },
                    new object[2] { "Аккумулятор",
                        item.Smartphone.Battery },
                    new object[2] { "Размеры",
                        item.Smartphone.Dimensions },
                    new object[2] { "Вес",
                        item.Smartphone.Weight },
                };

                return this.View(obj);
            }
            else
            {
                return this.View();
            }
        }
        public ActionResult Search(string query)
        {
            if (query == "*")//отобразить все товары
            {
                return this.Store(db.Items);
            }
            else if (query.Contains(Category.Laptop.ToString()))
            {
                return this.Store(db.Items.Where(x => x.Laptop != null));
            }
            else if (query.Contains(Category.Smartphone.ToString()))
            {
                return this.Store(db.Items.Where(x => x.Smartphone != null));
            }
            else
            {
                return this.Store(db.Items.Where(x => x.Name.Contains(query)));
            }
        }
    }
}