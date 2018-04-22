using Domain.Items;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PB.BL.Domain.Items
{
    [DataContract]
    [Table("tblOrganisation")]
    public class Organisation : Item
    {
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string SocialMediaLink { get; set; }
        [DataMember]
        public string IconURL { get; set; }
        [DataMember]
        public List<Person> People { get; set; }
        public virtual List<Record> Records { get; set; }

        public override string ToString()
        {
            return Name + " (Id: " + (ItemId) + ") - Aantal records: " + Records.Count;
        }
    }
}
