using Domain.Accounts;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PB.BL.Domain.Accounts
{
    [DataContract]
   
    public class Profile : IdentityUser
    {
        [DataMember]
        public string ProfileIcon { get; set; }
        [DataMember]
        public DateTime CreatedOn { get; set; }
        [DataMember]
        public UserData UserData { get; set; }
        [DataMember]
        public List<UserSetting> Settings { get; set; }
        public List<Dashboard> Dashboards { get; set; }
        public List<ProfileAlert> ProfileAlerts { get; set; }
        public List<Item> Subscriptions { get; set; }
        public List<Subplatform> AdminPlatforms { get; set; }
        public List<WeeklyReview> WeeklyReviews { get; set; }
        
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
