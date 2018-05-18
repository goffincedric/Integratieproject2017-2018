using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PB.BL.Domain.Items
{
    [DataContract]
    [Table("tblHashtag")]
    public class Hashtag
    {
        public Hashtag()
        {
        }

        public Hashtag(string hashTag)
        {
            HashTag = hashTag;
        }

        [Key] [DataMember] public int Id { get; set; }

        [DataMember] public string HashTag { get; set; }

        public List<Record> Records { get; set; }
    }
}