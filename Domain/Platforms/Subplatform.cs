using PB.BL.Domain.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using PB.BL.Domain.Items;

namespace PB.BL.Domain.Platform
{
    [Table("tblSubPlatform")]
    public class Subplatform
    {
        [Key]
        public int SubplatformId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string URL { get; set; }
        // string s1 = client.DownloadString("http://google.com"); 
        // Dit kijkt na of je site correct is en gooit 404 als die geen deftige pagina kan ophalen
        public DateTime DateOnline { get; set; }
        public Style Style { get; set; }
        public List<SubplatformSetting> Settings { get; set; }
        public List<Page> Pages { get; set; }
        public List<Profile> Admins { get; set; }
        public List<Item> Items { get; set; }

        public override string ToString()
        {
            return Name + " - " + URL;
        }
    }
}
