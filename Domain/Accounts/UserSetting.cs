using Domain.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Account
{
    [Table("tblUserSetting")]
    public class UserSetting
    {
        [Key, Column(Order = 0)]
        public Setting.Account SettingName { get; set; }
        public bool IsEnabled { get; set; }
        public string Value { get; set; }
        [Key, Column(Order = 2)]
        public string Username { get; set; }

        [Required]
        [ForeignKey("Username")]
        public Profile Profile { get; set; }
    }

    
}
