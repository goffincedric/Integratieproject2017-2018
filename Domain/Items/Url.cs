using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Items
{

  [Table("tblUrl")]
  public class Url
  {


    [Key] public int Id { get; set; }

    public string Link { get; set; }

    public List<Record> records { get; set; }

    public Url()
    {

    }

    public Url(string link)
    {
      Link = link;
    }
  }
}
