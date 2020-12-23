using AspShop.Models.Items;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspShop.Models
{
    public class AppUser : IdentityUser
    {
        [Description("Id пользователя")]
        public override string Id { get; set; }
        [Description("Имя пользователя")]
        public override string UserName { get; set; }
        [Description("Email пользователя")]
        public override string Email { get; set; }
        [Description("Номер телефона")]
        public override string PhoneNumber { get; set; }
        [Description("Пароль")]
        public string ModelPassword { get; set; }
        [Description("Дата регистрации")]

        public DateTime RegDate { get; set; }
        public virtual Customer Customer { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}