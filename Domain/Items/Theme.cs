using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PB.BL.Domain.Items
{
    [DataContract]
    [Table("tblTheme")]
    public class Theme : Item
    {
        [DataMember]
        public string Description { get; set; }

        public virtual List<Record> Records { get; set; }

        public override string ToString()
        {
            return Name + " (Id: " + (ItemId) + ") - Aantal records: " + Records.Count;
        }
    }
}
