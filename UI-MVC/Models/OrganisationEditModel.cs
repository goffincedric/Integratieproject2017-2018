using System.ComponentModel.DataAnnotations;
using System.Web;

namespace UI_MVC.Models
{
    public class OrganisationEditModel
    {
        public int ItemId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string FullName { get; set; }
        public string SocialMediaLink { get; set; }
        public bool IsTrending { get; set; }
        public int? ThemeId { get; set; }
        public HttpPostedFileBase file { get; set; }
    }
}