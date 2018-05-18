using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models
{
    public class SeedIntervals
    {
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int SEED_INTERVAL_HOURS { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int ALERT_GENERATION_INTERVAL_HOURS { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int SEND_WEEKLY_REVIEWS_INTERVAL_DAYS { get; set; }
    }
}