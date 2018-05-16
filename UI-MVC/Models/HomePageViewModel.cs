using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models
{
    public class HomePageViewModel
    {

        public string BannerTitle { get; set; }

        public string BannerTextSub1 { get; set; }

        public string BannerTextSub2 { get; set; }

        public string Call_to_action { get; set; }
    }
}