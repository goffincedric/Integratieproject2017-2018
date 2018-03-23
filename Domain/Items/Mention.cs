using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Items
{
  [Table("tblMention")]
  public class Mention
  {
    [Key]
    public int Number { get; set; }   
    public string Name { get; set; }

    
    public List<Record> records { get; set; }


    public Mention(string name)
    {
      Name = name;
    }

    public override string ToString()
    {
      return Name;
    }
  }
}
