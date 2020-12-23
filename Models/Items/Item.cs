using AspShop.Helpers.Attributes;
using AspShop.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspShop.Models.Items
{
    public class Item
    {
        public Item()
        {
            Id = Guid.NewGuid();
        }
        [Description("Id товара")]
        public Guid Id { get; set; }//Первичный ключ определяющий запись в таблице. Не пустое.
        [Description("Код товара"), Required(ErrorMessage = "Требуется поле Code")]
        public string Code { get; set; }//Код товара. Содержит данные формата «ХХ-XXХХ-YYXX» где Х–число, Y - заглавная буква английского алфавита. Не пустое.
        [Description("Название товара")]
        public string Name { get; set; }//Наименование товара.
        [Description("Стоимость")]
        public int Price { get; set; }//Цена за единицу.
        [Description("Категория")]
        public string Category { get; set; }//Категория товара.
        [Description("Фото")]
        public string Image { get; set; }
        [Description("Производитель")]
        public string Brand { get; set; }
        [Description("Описание")]
        public string Description { get; set; }
        public virtual Laptop Laptop { get; set; }
        public virtual Smartphone Smartphone { get; set; }
    }
}