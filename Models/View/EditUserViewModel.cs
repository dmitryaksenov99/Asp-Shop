using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspShop.Models.View
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerCode { get; set; }
        public int CustomerDiscount { get; set; }
    }
}