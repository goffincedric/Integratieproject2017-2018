using Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dashboards
{
  public class Comparison
  {
    public readonly int MAX_COMPARISONS = 10;
    public UserType Type { get; set; }
    public List<Item> ComparisonItems;

  }
}
