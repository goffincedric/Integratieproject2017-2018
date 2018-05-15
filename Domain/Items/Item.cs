using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Platform;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using PB.BL.Domain.Accounts;

namespace PB.BL.Domain.Items
{
    [DataContract]
    [Table("tblItem")]
    public class Item
    {
        [Key]
        [DataMember]
        public int ItemId { get; set; }
        [DataMember]
        [Required]
        public string Name { get; set; }
        [DataMember]
        public bool IsTrending { get; set; }
        [DataMember]
        public string IconURL { get; set; }

        public virtual List<Subplatform> SubPlatforms { get; set; }
        public List<Keyword> Keywords { get; set; }
        public List<Element> Elements { get; set; }
        public List<Profile> SubscribedProfiles { get; set; }
        public List<Alert> Alerts { get; set; }
    }
}
