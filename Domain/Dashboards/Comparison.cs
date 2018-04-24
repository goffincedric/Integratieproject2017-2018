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
        public static readonly int MAX_COMPARISONS = 10;
        public UserType Type { get; set; }

        public List<Item> Items { get; set; }
        public List<Element> Elements { get; set; }
    }
}
