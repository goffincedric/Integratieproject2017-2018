using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Items
{
  public class Organisation : Item
  {
    public String Name { get; set; }
    public String SocialMediaLink { get; set; }
    public String IconURL { get; set; }
    public List<Person> People { get; set; }
  }
}
