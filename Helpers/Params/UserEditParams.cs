using AspShop.Helpers.App;
using AspShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspShop.Helpers.Params
{
    public class UserEditParams
    {
        public List<Type> ModelsForEdit { get; set; }
        public FormCollection InputCollection { get; set; }
        public AppUserManager UserManager { get; set; }
        public bool IsForCreate { get; set; } = false;
    }
}