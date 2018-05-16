using PB.BL.Domain.Accounts;
using System;
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
        public int TrendingScore { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Level { get; set; }
        [DataMember]
        public string SocialMediaLink { get; set; }
        [DataMember]
        public string Site { get; set; }
        [DataMember]
        public string TwitterName { get; set; }
        [DataMember]
        public string Position { get; set; }
        [DataMember]
        public string District { get; set; }
        [DataMember]
        public string Gemeente { get; set; }
        [DataMember]
        public string Postalcode { get; set; }
        [DataMember]
        public Gender Gender { get; set; }
        [DataMember]
        public Organisation Organisation { get; set; }
        [DataMember]
        public DateTime DateOfBirth { get; set; }
        public virtual List<Record> Records { get; set; }
        public List<Theme> Themes { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Person person &&
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
