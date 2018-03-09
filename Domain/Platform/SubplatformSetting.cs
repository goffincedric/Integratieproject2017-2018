using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Platform
{
  public class SubplatformSetting
  {
    [Key]
    public string SettingName { get; set; }
    public bool IsEnabled { get; set; }
    public int Value { get; set; }
    
  }
}
