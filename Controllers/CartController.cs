using AspShop.Helpers;
using AspShop.Helpers.App;
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
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AspShop.Controllers
{
    public class CartController : Controller
    {
        public AppDbContext db = new AppDbContext();//Экземпляр контекста БД.

        public async Task<ActionResult> Index()
        {
            ViewBag.Viewport = Layout.GetViewport();

            try
            {
                var cart = await Cart();//Получаем коризину текущего пользователя.

                var cartModel = new List<CartViewModel>();//Создаём коллекцию для отображения товаров, которые находятся в корзине.

                var cartItems = db.OrderItems.Where(x => x.OrderId == cart.Id);//Получаем элементы корзины.

                foreach (var cartItem in cartItems)
                {
                    var item = db.Items.FirstOrDefault(x => x.Id == cartItem.ItemId);//Получаем товар, где x.Id - Id товара в списке всех товаров.

                    var cm = new CartViewModel//Создаём модель отображения товаров в корзине.
                    {
                    //    CustomerId = cart.CustomerId,//Id покупателя.
                        CartId = cartItem.OrderId,//Id корзины.
                        ItemId = cartItem.ItemId,//Id товара.
                        ItemsCount = cartItem.ItemsCount,//Количество товара.
                        ItemPrice = cartItem.ItemPrice,//Цена товара.
                        Image = item.Image,//Изображение товара.
                        Name = item.Name//Название товара.
                    };

                    cartModel.Add(cm);//Добавляем модель корзины в список моделей для отображения.
                }

                return this.View(cartModel);
            }
            catch (Exception ex)
            {
                Err.Show(ex, true);
                return this.Direct();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Spin(Guid itemId, Guid cartId, int itemsCount, string spinType)//Метод обновления количества товаров в корзине.
        {
            switch (spinType)//Если количество товаров увеличивается.
            {
                case "SpinUp":      
                    db.OrderItems.First(x => x.OrderId == cartId && x.ItemId == itemId).ItemsCount += 1;
                    break;
                case "SpinDown"://Если количество товаров уменьшается.
                    if (itemsCount > 0) db.OrderItems.First(x => x.ItemId == itemId).ItemsCount -= 1;
                    break;
                default:
                    break;
            }

            await db.SaveChangesAsync();//Сохраняем изменения в БД.
            this.GetCmp<Label>("cartCost").Text = string.Format("К оплате: {0}$", CartCost());

            return this.Direct();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddToCart(Guid itemId)
        {
            try
            {
                var cart = await Cart();

                var item = db.Items.FirstOrDefault(x => x.Id == itemId);//Находим товар по id.

                var orderExists = db.OrderItems.ToList().Exists(x => x.ItemId == item.Id && x.OrderId == cart.Id);//Проверяем наличие товара в корзине.

                if (orderExists)//Если товар есть в корзине, добавляем к его количеству единицу.
                {
                    db.OrderItems.First(x => x.ItemId == item.Id && x.OrderId == cart.Id).ItemsCount += 1;
                }
                else//Если товара нет в корзине, создаём новый товар.
                {
                    db.OrderItems.Add(new OrderItem
                    {
                        OrderId = cart.Id,
                        ItemId = item.Id,
                        ItemPrice = item.Price,
                        ItemsCount = 1
                    });

                    Msg.Show("Товар добавлен в корзину");
                }

                var cart2 = db.Orders.First(x => x.CustomerId == CurrentUser.Customer.Id && x.OrderDate == null);//Получаем корзину (order, у которого отсутствует дата заказа)
                var cartItems = db.OrderItems.Where(x => x.OrderId == cart2.Id);

                foreach (var cartItem in cartItems)
                {
                    cart2.Cost += cartItem.ItemPrice * cartItem.ItemsCount;
                }

                await db.SaveChangesAsync();//Сохраняем изменения в БД.
            }
            catch (Exception ex)
            {
                Err.Show(ex, true);
            }

            return this.Direct();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid itemId)
        {
            try
            {
                var order = db.OrderItems.First(x => x.ItemId == itemId);
                db.OrderItems.Remove(order);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Err.Show(ex, true);
            }

            return this.Direct();
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
        private double CartCost()
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
        private async Task<Order> Cart()
        {

            var cartExists = db.Orders.ToList().Exists(x => x.CustomerId == CurrentUser.Customer.Id && x.OrderDate == null);//Проверяем, существует ли корзина для текущего покупателя.

            if (cartExists)
            {
                return db.Orders.First(x => x.CustomerId == CurrentUser.Customer.Id && x.OrderDate == null);
            }
            else
            {
                var cart = db.Orders.Add(new Order//Создаём новый Order
                {
                    CustomerId = CurrentUser.Customer.Id
                });

                await db.SaveChangesAsync();

                return cart;
            }

        }
    }
}