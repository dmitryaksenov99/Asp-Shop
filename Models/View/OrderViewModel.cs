using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspShop.Models.View
{
    public class OrdersViewModel
    {
        [Required]
        public Guid CustomerId { get; set; }//ИД заказчика. Не пустое.
        [Required]
        public DateTime? OrderDate { get; set; }//Дата когда сделан заказ. Не пустое.
        public DateTime? ShipmentDate { get; set; }//Дата доставки.
        public int OrderNumber { get; set; }//Номер заказа.
        public string Status { get; set; }//Состояние заказа.
        public double Cost { get; set; }
        ////////////////////////////////
        [Required]
        public Guid OrderId { get; set; }//ИД заказа. Не пустое.
        [Required]
        public Guid ItemId { get; set; }//ИД товара. Не пустое.
        [Required]
        public int ItemsCount { get; set; }//Количество заказанного товара. Не пустое.
        [Required]
        public double ItemPrice { get; set; }//Цена за единицу. Не пустое.
        /////
        public string Name { get; set; }
        public string Image { get; set; }

    }
}