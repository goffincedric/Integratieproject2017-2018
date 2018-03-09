using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Account
{
  public class UserSetting
  {
    [Key]
    public string SettingName { get; set; }
    public bool IsEnabled { get; set; }
    public int Value { get; set; }

    [Required]
    [ForeignKey("Username")]
    public T Profile { get; set; }
  }
}
