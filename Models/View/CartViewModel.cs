using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspShop.Models.View
{
    public class CartViewModel
    {
        public Guid CartId { get; set; }//ИД заказа. Не пустое.
        public Guid ItemId { get; set; }//ИД товара. Не пустое.
        public int ItemsCount { get; set; }//Количество заказанного товара. Не пустое.
        public double ItemPrice { get; set; }//Цена за единицу. Не пустое.
        public string Name { get; set; }
        public string Image { get; set; }
    }
}