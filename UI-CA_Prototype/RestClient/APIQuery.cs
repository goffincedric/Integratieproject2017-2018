using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.JSONConversion
{
    public class APIQuery
    {
        [Required]
        public string Name { get; set; }
        public DateTime? Since { get; set; }
        public DateTime? Until { get; set; }
        public Dictionary<string, string[]> Themes { get; set; }

        public APIQuery()
        {

        }

        public APIQuery(string name)
        {
            this.Name = name;
        }
    }
}
