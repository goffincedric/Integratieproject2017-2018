using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Items
{
  [Table("tblPoliticianRecord")]
  public class Politician
  {
    [Key] public int Number { get; set; }

    [Required]
    public String Name { get; set; }

    public Politician(string name)
    {
      Name = name;

    }

    public override string ToString()
    {
      return Name;
    }
  }
}
