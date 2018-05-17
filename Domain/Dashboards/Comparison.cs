using PB.BL.Domain.Items;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Dashboards
{
    [Table("tblComparison")]
    public class Comparison
    {
        [Key]
        public int ComparisonId { get; set; }
        public virtual Item Item { get; set; }
        public virtual Element Element { get; set; }
    }
}
