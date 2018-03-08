using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Account
{
  public class UserSetting
  {
    public String SettingName { get; set; }
    public bool IsEnabled { get; set; }
    public int Value { get; set; }
  }
}
