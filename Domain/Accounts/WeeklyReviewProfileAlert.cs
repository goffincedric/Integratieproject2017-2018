using PB.BL.Domain.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
