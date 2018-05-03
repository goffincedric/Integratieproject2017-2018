using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_MVC.Models
{
    public class PersonEditModel
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public bool IsTrending { get; set; }
        public string IconURL { get; set; }
        public string SocialMediaLink { get; set; }
        public int? OrganisationId { get; set; }
    }
}