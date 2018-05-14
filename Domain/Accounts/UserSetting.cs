using PB.BL.Domain.Settings;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PB.BL.Domain.Accounts
{
    [Table("tblUserSetting")]

    public class UserSetting
    {
        [Key, Column(Order = 0)]
        [DataMember]
        public Setting.Account SettingName { get; set; }
        [DataMember]
        public bool IsEnabled { get; set; }
        [DataMember]
        public string Value { get; set; }
        [Key, Column(Order = 2)]
        public string UserId { get; set; }
        [DataMember]
        public bool boolValue { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public Profile Profile { get; set; }
    }

    
}
