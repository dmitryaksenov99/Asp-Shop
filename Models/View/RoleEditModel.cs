﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspShop.Models.View
{
    public class RoleEditModel
    {
        [Required]
        public string RoleName { get; set; }
        public string IdsToAdd { get; set; }
        public string IdsToDelete { get; set; }
    }
}