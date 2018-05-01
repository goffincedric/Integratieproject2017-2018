using PB.BL.Domain.Accounts;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public DateTime DateOnline { get; set; }

        public Style Style { get; set; }

        public List<SubplatformSetting> Settings { get; set; }
        public List<Page> Pages { get; set; }
        public List<Profile> Admins { get; set; }
        public List<Item> Items { get; set; }
        public List<Dashboard> Dashboards { get; set; }

        public override bool Equals(object obj)
        {
            var subplatform = obj as Subplatform;
            if (subplatform == null) return false;
            if (subplatform.URL.ToLower().Equals(URL.ToLower()) || 
                subplatform.SubplatformId == SubplatformId) return true;
            return false;
        }

        public override int GetHashCode()
        {
            return -1251312914 + EqualityComparer<string>.Default.GetHashCode(URL);
        }

        public override string ToString()
        {
            return Name + " - " + URL;
        }
    }
}
