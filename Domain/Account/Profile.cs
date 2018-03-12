﻿using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Account
{
  [Table("tblProfile")]
  public class Profile
  {
    [Key]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    [RegularExpression(@"^([\w -\.] +)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w -]+\.)+))([a - zA - Z]{2,4}|[0-9]{1,3})(\]?)$")]
    public string Email { get; set; }
    public Role Role { get; set; }
    public UserData UserData { get; set; }
    public List<UserSetting> Settings { get; set; }
    public Dictionary<SubPlatform, Dashboard> Dashboards { get; set; }
    public List<Alert> Alerts { get; set; }
    public Dictionary<Item, bool> Subscriptions { get; set; }

    public override string ToString()
    {
      return Username + " - " + Email;
    }
  }
}
