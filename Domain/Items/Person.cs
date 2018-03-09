using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Items
{
  public class Person : Item
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDay { get; set; }
    public string SocialMediaLink { get; set; }
    public string IconURL { get; set; }
    public Function Function { get; set; }
    public Organisation Organisation { get; set; }
  }
}
