using AspShop.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AspShop.Models.Items
{
    public class Order
    {
        public Order()
        {
            Id = Guid.NewGuid();
            OrderNumber = new Random().Next(0, int.MaxValue);
        }
        [Description("Id заказа")]
        public Guid Id { get; set; }//Первичный ключ определяющий запись в таблице. Не пустое.
        [Description("Id заказчика"), Required]
        public Guid CustomerId { get; set; }//ИД заказчика. Не пустое.
        [Description("Дата заказа")]
        public DateTime? OrderDate { get; set; }//Дата когда сделан заказ. Не пустое.
        [Description("Дата доставки")]
        public DateTime? ShipmentDate { get; set; }//Дата доставки.
        [Description("Номер заказа")]
        public int OrderNumber { get; set; }//Номер заказа.
        [Description("Статус")]
        public string Status { get; set; }//Состояние заказа.
        [Description("Комментарий")]
        public string Comment { get; set; }//Комментарий к заказу
        [Description("Стоимость")]
        public double Cost { get; set; }//Стоимость заказа
    }
}