using PB.BL.Domain.Settings;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Platform
{
    [Table("tblSubPlatformSetting")]
    public class SubplatformSetting
    {
        [Key, Column(Order = 0)]
        public virtual Setting.Platform SettingName { get; set; }
        [Key, Column(Order = 1)]
        public int SubplatformId { get; set; }

        public bool IsEnabled { get; set; }
        public string Value { get; set; }
        public virtual Subplatform Subplatform { get; set; }

        public override bool Equals(object obj)
        {
            return obj is SubplatformSetting setting &&
                   SettingName == setting.SettingName &&
                   EqualityComparer<Subplatform>.Default.Equals(Subplatform, setting.Subplatform);
        }

        public override int GetHashCode()
        {
            var hashCode = 645998418;
            hashCode = hashCode * -1521134295 + SettingName.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Subplatform>.Default.GetHashCode(Subplatform);
            return hashCode;
        }
    }
}
