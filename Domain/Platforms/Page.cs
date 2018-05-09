using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Platform
{
    [Table("tblPage")]
    public class Page
    {
        [Key]
        public int PageId { get; set; }
        [Required]
        public string PageName { get; set; }
        [Required]
        public string Title { get; set; }
        public List<Tag> Tags { get; set; }

        public int SubplatformId { get; set; }
        [Required]
        public Subplatform Subplatform { get; set; }

    }
}
