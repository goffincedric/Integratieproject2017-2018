using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Items
{

  [Table("tblHashtag")]
  public class Hashtag
  {
    [Key] public int Number { get; set; }
    public String tag { get; set; }

    public List<Record> records { get; set; }

    public Hashtag()
    {

    }

    public Hashtag(string tag)
    {
      this.tag = tag;
    }
  }
}
