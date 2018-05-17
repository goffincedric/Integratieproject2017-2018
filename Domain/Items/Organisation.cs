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
        [Required] [DataMember] public string FullName { get; set; }

        [DataMember] public string SocialMediaLink { get; set; }

        [DataMember] public virtual List<Person> People { get; set; }

        //[DataMember]
        public virtual List<Theme> Themes { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Organisation organisation &&
                   Name.ToLower().Equals(organisation.Name.ToLower()) &&
                   FullName.ToLower().Equals(organisation.FullName.ToLower());
        }

        public override int GetHashCode()
        {
            var hashCode = -792337166;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FullName);
            return hashCode;
        }

        public override string ToString()
        {
            return Name + ": " + FullName + " (Id: " + ItemId + ")";
        }
    }
}