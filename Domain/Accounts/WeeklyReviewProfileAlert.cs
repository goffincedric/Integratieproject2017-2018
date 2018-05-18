using System.ComponentModel.DataAnnotations.Schema;
using PB.BL.Domain.Accounts;

namespace Domain.Accounts
{
    [Table("tblWeeklyReviewProfileAlert")]
    public class WeeklyReviewProfileAlert
    {
        public int WeeklyReviewId { get; set; }
        public int ProfileAlertId { get; set; }

        public virtual WeeklyReview WeeklyReview { get; set; }
        public virtual ProfileAlert ProfileAlert { get; set; }
    }
}