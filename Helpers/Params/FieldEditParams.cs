using AspShop.Helpers.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspShop.Helpers.Params
{
    public class FieldEditParams
    {
        public List<Type> ModelsForEdit { get; set; }
        public FormCollection InputCollection { get; set; }
    }
}