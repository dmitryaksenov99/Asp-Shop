using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspShop.Helpers.Params
{
    public class FieldCreateParams
    {
        public int DescriptionHeight { get; set; } = 150;
        public int RegularHeight { get; set; } = 32;
        public string WidthSpec { get; set; } = "100%";
        public int PanelBodyPadding { get; set; } = 20;
        public string ButtonUrlAction { get; set; }
        public string ButtonText { get; set; }
        public List<object> ModelsForParse { get; set; }
        public bool IsForCreate { get; set; } = false;
    }
}