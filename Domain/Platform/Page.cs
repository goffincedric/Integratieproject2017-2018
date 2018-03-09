using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Platform
{
  public class Page
  {
    public string Title { get; set; }
    public string FaciconURL { get; set; }
    public List<Tag> Tags { get; set; }
  }
}
