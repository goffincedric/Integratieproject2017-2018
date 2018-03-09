using Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Platform
{
  public class SubPlatform
  {
    public String Name { get; set; }
    public String URL { get; set; }
    public String SourceAPI { get; set; }
    public String SiteIconURL { get; set; }
    public DateTime DateOnline { get; set; }
    public Style Style { get; set; }
    public List<SubplatformSetting> Settings { get; set; }
    public List<Page> Pages { get; set; }
    public List<Profile> Admins { get; set; }
  }
}
