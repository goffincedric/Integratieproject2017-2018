using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Platform
{
  public class Page
  {
    public String Title { get; set; }
    public String FaciconURL { get; set; }
    public List<Tag> Tags { get; set; }
  }
}
