using AspShop.Helpers;
using AspShop.Helpers.App;
using AspShop.Helpers.Enums;
using AspShop.Helpers.Messages;
using AspShop.Models;
using AspShop.Models.Items;
using AspShop.Models.View;
using Ext.Net;
using Ext.Net.MVC;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AspShop.Controllers
{
    public class OrdersController : Controller
    {
        public AppDbContext db = new AppDbContext();

        public string addTo = "rightContainer";
        public ActionResult Index()
        {
            ViewBag.Viewport = Layout.GetViewport();

            var orders = db.Orders.Where(x => x.CustomerId == CurrentUser.Customer.Id && x.OrderDate != null);

            var ordersModel = new List<OrdersViewModel>();

            foreach (var order in orders)
            {
                var orderItems = db.OrderItems.Where(x => x.OrderId == order.Id);

                var itemsCount = 0;
                foreach (var item in orderItems)
                {
                    itemsCount += item.ItemsCount;
                }

                var om = new OrdersViewModel
                {
                    OrderId = order.Id,
                    CustomerId = order.CustomerId,
                    OrderDate = order.OrderDate,
                    Cost = order.Cost,
                    ShipmentDate = order.ShipmentDate,
                    OrderNumber = order.OrderNumber,
                    ItemsCount = itemsCount
                };

                ordersModel.Add(om);
            }

            return this.View(ordersModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToOrders(string orderComment)
        {
            var cart = db.Orders.First(x => x.CustomerId == CurrentUser.Customer.Id && x.OrderDate == null);//Получаем корзину (order, у которого отсутствует дата заказа)
            cart.OrderDate = DateTime.Now;
            cart.Cost = OrderCost;
            cart.Status = "Новый";
            cart.Comment = orderComment;

            var cartItems = db.OrderItems.Where(x => x.OrderId == cart.Id);

            foreach (var item in cartItems)
            {
                cart.Cost += item.ItemPrice * item.ItemsCount;
            }

            db.SaveChanges();

            return RedirectToAction("Index", "Orders");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetSelectedOrder(Guid id)
        {
            var order = db.Orders.FirstOrDefault(x => x.Id == id);

            var panel = new Panel
            {

                Items =
                {
                    new Panel
                    {
                        BodyPadding = 20,
                        Items =
                        {
                            new Label
                            {
                                Text = "ID заказа: " + order.OrderNumber,
                                Cls = "orderId",
                                MarginSpec="0 0 20 0",                             
                            },
                            new Label
                            {
                                Html = string.Format("{0}Стоимость заказа: {2}{1}", "<p/>", "<p/>", order.Cost),
                            },
                            new Label
                            {
                                Html = string.Format("{0}Статус доставки: {2}{1}", "<p/>", "<p/>", order.Status),
                            },
                            new Label
                            {
                                Html = string.Format("{0}Адрес доставки: {2}{1}", "<p/>", "<p/>", CurrentUser.Customer.Address),
                            },
                            new Label
                            { 
                                Html = "Комметарий к заказу: ",
                            },
                            new Label
                            {
                                ID = "orderComment",
                                Text = order.Comment,
                                Listeners =
                                {
                                    AfterRender =
                                    {
                                        Handler = System.IO.File.ReadAllText(Server.MapPath("~/Scripts/userscripts/trimCommentInOrder.js"))
                                    }
                                }
                            }
                        }
                    },
                    
                    new GridPanel
                    {
                        Height = 500,
                        Scrollable = ScrollableOption.Vertical,
                        Store =
                        {                        
                            new Store
                            {
                                ID = "OrderStore",
                                Proxy =
                                {
                                    new AjaxProxy
                                    {
                                        Url = Url.Action("GetOrderedItems", "Orders"),
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
                                            new Parameter("__RequestVerificationToken", AntiForgeryField.GetField())
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
                                    Width = 203,
                                },
                                new Column
                                {
                                    DataIndex = "ItemsCount",
                                    Width = 70,
                                },
                            }
                        },
                        Listeners =
                        {
                            ItemClick =
                            {
                                Handler = "window.location.href = '/Item/Index?id=' + App.OrderStore.getAt(index).data.Id"
                            }
                        }
                    }
                }
            };

            this.GetCmp<Panel>(addTo).RemoveAll();
            panel.AddTo(this.GetCmp<Panel>(addTo));

            return this.Direct();
        }

        public ActionResult GetOrderedItems(Guid id)
        {
            var obj = new List<OrderViewModel>();
            var order = db.Orders.First(x => x.Id == id);

            var orderItems = db.OrderItems.Where(x => x.OrderId == order.Id);

            foreach (var orderItem in orderItems)
            {
                var item = db.Items.First(x => x.Id == orderItem.ItemId);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid orderId)
        {
            try
            {
                var order = db.Orders.First(x => x.Id == orderId);
                db.Orders.Remove(order);

                var orderItems = db.OrderItems.Where(x => x.OrderId == orderId);
                foreach (var item in orderItems)
                {
                    db.OrderItems.Remove(item);
                }

                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Err.Show(ex, true);
            }

            return this.Direct();
        }
        private double OrderCost
        {
            get
            {
                double cost = 0;

                var cart = db.Orders.First(x => x.CustomerId == CurrentUser.Customer.Id && x.OrderDate == null);//Получаем корзину (order, у которого отсутствует дата заказа)
                var cartItems = db.OrderItems.Where(x => x.OrderId == cart.Id);

                foreach (var item in cartItems)
                {
                    cost += item.ItemPrice * item.ItemsCount;
                }

                return cost;
            }
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
    }
}