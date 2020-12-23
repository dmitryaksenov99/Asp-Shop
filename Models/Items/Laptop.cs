using AspShop.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AspShop.Models.Items
{
    public class Laptop
    {
        public Laptop()
        {
            Id = Guid.NewGuid();
        }
        [Description("Id ноутбука")]
        public Guid Id { get; set; }
        [Description("Процессор")]
        public string Cpu { get; set; }
        [Description("Диагональ экрана")]
        public double ScreenDiagonal { get; set; }
        [Description("Объём ОЗУ")]
        public int RamSize { get; set; }
        [Description("Тип ОЗУ")]
        public RamType RamType { get; set; }
        [Description("ОС")]
        public string Os { get; set; }
        [Description("Объём внутренней памяти")]
        public string Storage { get; set; }
        [Description("Дополнительные функции")]
        public string AdditionalFatures { get; set; }
        [Description("Графический адаптер")]
        public string GraphicsAdapter { get; set; }
        [Description("Разъёмы")]
        public string Connectors { get; set; }
        [Description("Аккумулятор")]
        public string Battery { get; set; }
        [Description("Размеры")]
        public string Dimensions { get; set; }
        [Description("Вес")]
        public double Weight { get; set; }
    }
}