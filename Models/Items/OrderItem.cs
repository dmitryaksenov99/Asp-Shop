using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspShop.Models.Items
{
    public class OrderItem
    {
        public OrderItem()
        {
            Id = Guid.NewGuid();
        }
    //    [Description("Id элемента заказа")]
        public Guid Id { get; set; }//Первичный ключ определяющий запись в таблице. Не пустое.
    //    [Description("Id заказа")]
        public Guid OrderId { get; set; }//ИД заказа. Не пустое.
    //    [Description("Id товара"), Required]
        public Guid ItemId { get; set; }//ИД товара. Не пустое.
      //  [Description("Количество товара"), Required]
        public int ItemsCount { get; set; }//Количество заказанного товара. Не пустое.
      //  [Description("Цена за единицу"), Required]
        public double ItemPrice { get; set; }//Цена за единицу. Не пустое.
    }
}