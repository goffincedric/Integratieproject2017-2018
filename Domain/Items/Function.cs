using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Items
{
  public class Function
  {
    [Key]
    public int FunctionId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Since { get; set; }
  }
}
