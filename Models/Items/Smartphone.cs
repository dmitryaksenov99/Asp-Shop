using AspShop.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AspShop.Models.Items
{
    public class Smartphone
    {
        public Smartphone()
        {
            Id = Guid.NewGuid();
        }
        [Description("Id смартфона")]
        public Guid Id { get; set; }
        [Description("Диагональ экрана")]
        public double DisplayDiagonal { get; set; }
        [Description("Тип экрана")]
        public DisplayType DisplayType { get; set; }
        [Description("Объём ОЗУ")]
        public int RamSize { get; set; }
        [Description("Объём внутренней памяти")]
        public string Storage { get; set; }
        [Description("Аккумулятор")]
        public string Battery { get; set; }
        [Description("Размеры")]
        public string Dimensions { get; set; }
        [Description("Вес")]
        public double Weight { get; set; }
    }
}