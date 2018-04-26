using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Dashboards
{
    [Table("tblElement")]
    public class Element
    {
        [Key]
        public int ElementId { get; set; }
        [Required]
        public int X { get; set; }
        [Required]
        public int Y { get; set; }
        [Required]
        public int Width { get; set; }
        [Required]
        public int Height { get; set; }
        public bool IsDraggable { get; set; }
        
        public int ZoneId { get; set; }
        [Required]
        public Zone Zone { get; set; }
        public Comparison Comparison { get; set; }
    }
}
