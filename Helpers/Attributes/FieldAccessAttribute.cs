using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspShop.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class FieldAccessAttribute : Attribute
    {
        private string _roles;
        private string[] _rolesSplit = new string[0];
        public string Roles
        {
            get
            {
                return _roles ?? string.Empty;
            }
            set
            {
                _roles = value;
                _rolesSplit = value.Split(',');
            }
        }
        public FieldAccessAttribute(string roles)
        {
            this.Roles = roles;
        }
    }
}