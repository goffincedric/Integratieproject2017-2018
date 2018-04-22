using PB.BL.Domain.Account;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Platform;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

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
        public string Name { get; set; }
        [DataMember]
        public bool IsHot { get; set; }

        public List<Subplatform> SubPlatforms { get; set; }
        public List<Keyword> Keywords { get; set; }
        public List<Comparison> Comparisons { get; set; }
        public List<Profile> SubscribedProfiles { get; set; }
        public List<Alert> Alerts { get; set; }
    }
}
