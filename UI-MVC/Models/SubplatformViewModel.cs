using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UI_MVC.Models
{
    public class SubplatformViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Url { get; set; }
        public string SourceAPI { get; set; }
    }
}