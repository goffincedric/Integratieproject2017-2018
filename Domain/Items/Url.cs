using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Items
{

    [Table("tblUrl")]
  public class Url
  {
    [Key]
    public int Id { get; set; }
    public string Link { get; set; }

    public List<Record> Records { get; set; }

    public Url()
    {

    }

    public Url(string link)
    {
      Link = link;
    }
  }
}
