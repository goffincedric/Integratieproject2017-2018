using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_MVC.Models
{
    public class ThemeEditModel
    {
        public int ItemId { get; set; }
        public String Name { get; set; }
        public bool IsTrending { get; set; }
        public string Description { get; set; }
        public HttpPostedFileBase file { get; set; }
    }
}