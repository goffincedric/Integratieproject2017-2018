using PB.BL.Domain.Accounts;
using PB.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Accounts
{
    [Table("tblWeeklyReview")]
    public class WeeklyReview
    {
        [Key]
        public int WeeklyReviewId { get; set; }
        public string UserId { get; set; }
        public int TopPersonId { get; set; }
        [Required]
        public string TopPersonText { get; set; }
        [Required]
        public DateTime TimeGenerated { get; set; }
        [Required]
        public Profile Profile { get; set; }

        [Required]
        public virtual List<WeeklyReviewProfileAlert> WeeklyReviewsProfileAlerts { get; set; }
    }
}
