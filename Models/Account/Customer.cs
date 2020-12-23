using AspShop.Models.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AspShop.Models
{
    public class Customer
    {
        [Description("Id покупателя")]
        public Guid Id { get; set; } = Guid.NewGuid();//Первичный ключ определяющий запись в таблице. Не пустое.
        [Description("Имя покупателя"), Required(ErrorMessage = "Требуется поле Name")]
        public string Name { get; set; }//Наименование заказчика. Не пустое.
        [Description("Код покупателя"), Required(ErrorMessage = "Требуется поле Code")]
        public string Code { get; set; }//Код заказчика. Содержит данные формата «ХХХХ-ГГГГ» где Х–число, ГГГГ–год в которомзарегистрирован заказчик. Не пустое.
        [Description("Адрес покупателя")]
        public string Address { get; set; }//Адрес заказчика.
        [Description("Скидка покупателя")]
        public double Discount { get; set; }//% скидки для заказчика. 0 или null – означает что, скидка не распространяется.
    }
}