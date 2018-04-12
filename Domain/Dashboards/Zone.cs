using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Dashboards
{
    [Table("tblZone")]
    public class Zone
    {
        [Key]
        public int ZoneId { get; set; }
        public string Name { get; set; }
        public bool IsFull { get; set; }
        public readonly int MAX_ELEMENTS;
        public int Width { get; set; }
        public int Height { get; set; }
        public int Position { get; set; }

        public List<Element> Elements { get; set; }
    }
}
