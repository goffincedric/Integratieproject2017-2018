using PB.BL.Domain.Settings;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Platform
{
    [Table("tblSubPlatformSetting")]
  public class SubplatformSetting
  {
    [Key]
    public Setting.Platform SettingName { get; set; }
    public bool IsEnabled { get; set; }
    public string Value { get; set; }

  }
}
