using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Dashboards
{
    [Table("tblGraph")]
    public class Graph : Element
    {
        public GraphType GraphType { get; set; }
    }
}
