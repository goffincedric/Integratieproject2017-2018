using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Platform
{
    [Table("tblStyle")]
    public class Style
    {
        [Key] public int StyleId { get; set; }

        public string Font { get; set; }
        public string ThemeHEXColor1 { get; set; }
        public string ThemeHEXColor2 { get; set; }
    }
}