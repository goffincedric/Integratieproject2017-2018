using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Dashboards
{
  public class Zone
  {
    public string Name { get; set; }
    public bool IsFull { get; set; }
    public readonly int MAX_ELEMENTS;
    public int Width { get; set; }
    public int Height { get; set; }
    public int Position { get; set; }

  }
}
