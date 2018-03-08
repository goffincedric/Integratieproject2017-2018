using Domain.Dashboards;
using Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Account
{
  public class Profile
  {
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
    public UserData UserData { get; set; }
    public List<UserSetting> Settings { get; set; }
    public List<Dashboard> Dashboards { get; set; }
    public List<Alert> Alerts { get; set; }
    public List<Item> Subscriptions { get; set; }
  }
}
