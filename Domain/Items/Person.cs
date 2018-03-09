using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Items
{
  public class Person : Item
  {
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public DateTime BirthDay { get; set; }
    public String SocialMediaLink { get; set; }
    public String IconURL { get; set; }
    public Function Function { get; set; }
    public Organisation Organisation { get; set; }
  }
}
