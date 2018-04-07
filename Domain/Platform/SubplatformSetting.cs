using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Platform
{
  [Table("tblSubPlatformSetting")]
  public class SubplatformSetting
  {
    [Key]
    public string SettingName { get; set; }
    public bool IsEnabled { get; set; }
    public int Value { get; set; }
    
  }
}
