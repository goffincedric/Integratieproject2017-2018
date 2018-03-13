using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Items
{

  [Table("tblHashtagRecord")]
  public class Hashtag
  {
    [Key] public int Number { get; set; }
    public String tag { get; set; }

    public Hashtag(string tag)
    {
      this.tag = tag;
    }
  }
}
