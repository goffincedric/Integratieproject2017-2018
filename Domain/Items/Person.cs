using Domain.Items;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PB.BL.Domain.Items
{
    [DataContract]
    [Table("tblPerson")]
    public class Person : Item
    {
        [DataMember]
        public string SocialMediaLink { get; set; }
        [DataMember]
        public string IconURL { get; set; }
        [DataMember]
        public Function Function { get; set; }
        [DataMember]
        public Organisation Organisation { get; set; }
        public virtual List<Record> Records { get; set; }

        public override string ToString()
        {
            return Name + " (Id: " + (ItemId) + ") - Aantal records: " + Records.Count;
        }
    }
}
