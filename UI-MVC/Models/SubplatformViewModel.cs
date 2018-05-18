using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models
{
    public class SubplatformViewModel
    {
        [Required] public string Name { get; set; }
        public string Url { get; set; }
        public string SourceAPI { get; set; }
    }
}