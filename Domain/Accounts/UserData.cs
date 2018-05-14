using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PB.BL.Domain.Accounts
{
    [Table("tblUserData")]
    [DataContract]
    public class UserData
    {
        [Key]
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        //public string Telephone { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public string City { get; set; }
        // public DateTime BirthDate { get; set; }
        [DataMember]
        public Province Province { get; set; }
        //public Gender Gender { get; set; }

        [Required]
     
        public Profile Profile { get; set; }
    }
}
