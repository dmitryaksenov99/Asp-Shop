using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspShop.Models.View
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public int ItemsCount { get; set; }
    }
}