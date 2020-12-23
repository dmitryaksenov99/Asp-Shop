using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AspShop.Helpers
{
    public static class AntiForgeryField//токен, генерируемый внутри проекта для предотвращения внешних запросов к методам контроллера
    {
        public static string GetField()
        {
            string input = System.Web.Helpers.AntiForgery.GetHtml().ToString();
            var match = Regex.Match(input, "name=\"(.+?)\"");//нужна ли эта строка?
            match = Regex.Match(input, "value=\"(.+?)\"");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return "null";
            }
        }
    }
}