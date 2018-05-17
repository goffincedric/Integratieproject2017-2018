using System;
using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models
{
    public class SubplatformSettingViewModel
    {
        public String SiteName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int recordsBijhouden { get; set; }

        public string APIsource { get; set; }
        public string SocialSource { get; set; }
        public string SocialSourceUrl { get; set; }
        public string Theme { get; set; }
    }
}