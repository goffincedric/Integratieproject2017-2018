using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Settings;

namespace PB.BL.Domain.Platform
{
  [Table("tblSubPlatformSetting")]
  public class SubplatformSetting
  {
    //Key aanpassen naar SettingName + SubplatformId
    [Key]
    public Setting.Platform SettingName { get; set; }
    public bool IsEnabled { get; set; }
    public string Value { get; set; }

  }
}
