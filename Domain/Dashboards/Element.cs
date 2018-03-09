using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Dashboards
{
  public class Element
  {
    [Key]
    public int ElementId { get; set; }
    public int Position { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string Title { get; set; }
    public bool IsDraggable { get; set; }

  }
}
