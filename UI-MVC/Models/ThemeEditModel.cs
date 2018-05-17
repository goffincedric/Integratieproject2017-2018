using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UI_MVC.Models
{
    public class ThemeEditModel
    {
        public int ItemId { get; set; }
        [Required] public String Name { get; set; }
        public bool IsTrending { get; set; }
        public string Description { get; set; }
        [Required] public int? KeywordId { get; set; }
        public HttpPostedFileBase file { get; set; }
    }
}