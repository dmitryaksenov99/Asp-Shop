using Ext.Net;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace AspShop.Helpers.Messages
{
    public static class Err
    {
        public static void Show(Exception ex, bool full = false)
        {
            string header = "Ошибка";
            var errors = new List<string>();

            if (ex.GetType() == typeof(DbEntityValidationException))
            {
                foreach (var eve in ((DbEntityValidationException)ex).EntityValidationErrors)
                {
                    errors.Add(string.Format("Объект типа \"{0}\" в состоянии \"{1}\" имеет следующие ошибки валидации:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        errors.Add(string.Format("- Свойство: \"{0}\", Ошибка: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
            }
            else if (full)
            {
                errors.Add(ex.ToString());
            }
            else
            {
                errors.Add(ex.Message.ToString());
            }

            X.Msg.Alert(header, string.Join(Environment.NewLine, errors)).Show();
        }
    }
}