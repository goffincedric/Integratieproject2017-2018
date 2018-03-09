using Domain.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Items
{
  public class Item
  {
    public bool IsHot { get; set; }
    public List<SubPlatform> SubPlatforms { get; set; }
    public List<Keyword> Keywords { get; set; }
  }
}
