using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Platform
{
  public class Tag
  {
    public string HtmlId { get; set; }
    public List<string> HtmlClasses { get; set; }
    public string NameObject { get; set; }
    public string Text { get; set; }
  }
}
