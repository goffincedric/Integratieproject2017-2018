using PB.BL.Domain.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Items
{
  public class Item
  {
    [Key]
    public int ItemId { get; set; }
    public bool IsHot { get; set; }
    public List<SubPlatform> SubPlatforms { get; set; }
    public List<Keyword> Keywords { get; set; }

    [Required]
    [ForeignKey("ComparisonId")]
    public
  }
}
