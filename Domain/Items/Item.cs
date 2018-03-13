using PB.BL.Domain.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Items;

namespace PB.BL.Domain.Items
{
  [Table("tblItem")]
  public class Item
  {
    [Key]
    public int ItemId { get; set; }
    public bool IsHot { get; set; }
    public List<SubPlatform> SubPlatforms { get; set; }
    public List<Record> Records;
    public List<Keyword> Keywords { get; set; }
  }
}
