using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Account;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace PB.BL.Domain.Account
{
    public class Profile : IdentityUser
    {
        public string ProfileIcon { get; set; }

        public UserData UserData { get; set; }
        public List<UserSetting> Settings { get; set; }
        public List<Dashboard> Dashboards { get; set; }
        public List<Alert> Alerts { get; set; }
        public List<Item> Subscriptions { get; set; }
        public List<Subplatform> AdminPlatforms { get; set; }
        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Profile> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }

        public override string ToString()
        {
            return UserName + " - " + Email;
        }
    }
}
