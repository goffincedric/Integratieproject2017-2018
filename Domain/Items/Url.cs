using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Items
{

  [Table("tblUrl")]
  public class Url
  {


    [Key] public int Number { get; set; }

    public string Link { get; set; }

    public Url(string link)
    {
      Link = link;
    }
  }
}
