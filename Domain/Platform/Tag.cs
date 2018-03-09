using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Platform
{
  public class Tag
  {
    public String HtmlId { get; set; }
    public List<String> HtmlClasses { get; set; }
    public String NameObject { get; set; }
    public String Text { get; set; }
  }
}
