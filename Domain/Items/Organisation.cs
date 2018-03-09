using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Items
{
  public class Organisation : Item
  {
    public string Name { get; set; }
    public string SocialMediaLink { get; set; }
    public string IconURL { get; set; }
    public List<Person> People { get; set; }
  }
}
