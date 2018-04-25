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

        public override bool Equals(object obj)
        {
            var person = obj as Person;
            return person != null &&
                   Name.ToLower().Equals(person.Name.ToLower());
        }

        public override int GetHashCode()
        {
            var hashCode = 696847547;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }

        public override string ToString()
        {
            return Name + " (Id: " + (ItemId) + ") - Aantal records: " + Records.Count;
        }
    }
}
