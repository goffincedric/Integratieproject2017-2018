using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Items
{
  [Table("tblKeyword")]
  public class Keyword
  {
    [Key]
    public int KeywordId { get; set; }
    public string Name { get; set; }
    public List<Item> Items { get; set; }
  }
}
