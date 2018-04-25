using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string Abbreviation { get; set; }
        [DataMember]
        public string SocialMediaLink { get; set; }
        [DataMember]
        public string IconURL { get; set; }
        [DataMember]
        public List<Person> People { get; set; }
        public virtual List<Record> Records { get; set; }

        public override bool Equals(object obj)
        {
            var organisation = obj as Organisation;
            return organisation != null &&
                Name.ToLower().Equals(organisation.Name.ToLower()) &&
                Abbreviation.ToLower().Equals(organisation.Abbreviation.ToLower());
        }
        
        public override int GetHashCode()
        {
            var hashCode = -792337166;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            return hashCode;
        }

        public override string ToString()
        {
            return Name + " (Id: " + (ItemId) + ") - Aantal records: " + Records.Count;
        }
    }
}
