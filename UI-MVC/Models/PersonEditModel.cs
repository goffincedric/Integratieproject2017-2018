using System;
using System.Web;

namespace UI_MVC.Models
{
    public class PersonEditModel
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public bool IsTrending { get; set; }
        public string SocialMediaLink { get; set; }
        public int? OrganisationId { get; set; }
        public String Gemeente { get; set; }
        public String District { get; set; }
        public String Level { get; set; }
        public String TwitterName { get; set;  }
        public String Site { get; set; }

        public HttpPostedFileBase file { get; set; }
    }
}