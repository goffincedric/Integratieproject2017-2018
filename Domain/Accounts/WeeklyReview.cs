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
    [Table("tblWeeklyAlerts")]
    public class WeeklyReview
    {
        [Key]
        public int WeeklyReviewId { get; set; }
        public string UserId { get; set; }
        [Required]
        public int TopPersonId { get; set; }
        [Required]
        public DateTime TimeGenerated { get; set; }
        [Required]
        public Profile Profile { get; set; }

        [Required]
        public List<WeeklyReviewProfileAlerts> WeeklyReviewsProfileAlerts { get; set; }
    }
}
