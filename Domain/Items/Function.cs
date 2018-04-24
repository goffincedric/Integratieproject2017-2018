using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Items
{
    [Table("tblFunction")]
  public class Function
  {
    [Key]
    public int FunctionId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Since { get; set; }
  }
}
