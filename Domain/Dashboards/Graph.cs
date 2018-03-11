using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Dashboards
{
  [Table("tblGraph")]
  public class Graph : Element
  {
    public GraphType GraphType { get; set; }
  }
}
