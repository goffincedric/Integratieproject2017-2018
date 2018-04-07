using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Platform
{
  [Table("tblPage")]
  public class Page
  {
    [Key]
    public int PageId { get; set; }
    public string Title { get; set; }
    public string FaviconURL { get; set; }
    public List<Tag> Tags { get; set; }
  }
}
