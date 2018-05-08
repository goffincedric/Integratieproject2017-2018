using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_MVC.Models
{
    public class UserSettingViewModel
    {
        public bool WANTS_EMAIL_NOTIFICATIONS { get; set; }
        public bool WANTS_ANDROID_NOTIFICATIONS { get; set; }
        public bool WANTS_SITE_NOTIFICATIONS { get; set; }
        public bool WANTS_WEEKLY_REVIEW_VIA_MAIL { get; set; }
    }
}