using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Items
{
  [Table("tblOrganisation")]
  public class Organisation : Item
  {
    public string SocialMediaLink { get; set; }
    public string IconURL { get; set; }
    public List<Person> People { get; set; }
  }
}
