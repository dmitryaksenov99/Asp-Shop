using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspShop.Helpers.Messages
{
    public class Msg
    {
        public static void Show(string text = "", string header = "Информация")
        {
            X.Msg.Alert(header, text).Show();
        }
    }
}