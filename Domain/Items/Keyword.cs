using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Items
{
  public class Keyword
  {
    [Key]
    public int KeywordId { get; set; }
    public string Name { get; set; }
    public List<Item> Items { get; set; }
  }
}
