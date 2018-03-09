using PB.BL.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Platform
{
  public class SubPlatform
  {
    public string Name { get; set; }
    public string URL { get; set; }
        // string s1 = client.DownloadString("http://google.com"); 
        // Dit kijkt na of je site correct is en gooit 404 als die geen deftige pagina kan ophalen
        public string SourceAPI { get; set; }
    public string SiteIconURL { get; set; }
    public DateTime DateOnline { get; set; }
    public Style Style { get; set; }
    public List<SubplatformSetting> Settings { get; set; }
    public List<Page> Pages { get; set; }
    public List<Profile> Admins { get; set; }
  }
}
