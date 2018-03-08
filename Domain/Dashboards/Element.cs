using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dashboards
{
  public class Element
  {
    public int Position { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string Title { get; set; }
    public bool IsDraggable { get; set; }

  }
}
