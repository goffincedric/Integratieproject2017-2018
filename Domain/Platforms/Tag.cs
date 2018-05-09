using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Platform
{
    [Table("tblTag")]
    public class Tag
    {
        [Key]
        public int TagId { get; set; }
        [Required]
        public string CssName { get; set; }
        [Required]
        public string NameObject { get; set; }
        public string Text { get; set; }

        public int PageId { get; set; }
        [Required]
        public Page Page { get; set; }
    }
}
