using PB.BL.Domain.Account;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Platform;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Items
{
    [Table("tblItem")]
    public class Item
    {
        [Key]
        public int ItemId { get; set; }
        public string Name { get; set; }
        public bool IsHot { get; set; }

        public List<Subplatform> SubPlatforms { get; set; }
        public List<Keyword> Keywords { get; set; }
        public List<Comparison> Comparisons { get; set; }
        public List<Profile> SubscribedProfiles { get; set; }
    }
}
