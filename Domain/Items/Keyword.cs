using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PB.BL.Domain.Items
{
    [DataContract]
    [Table("tblKeyword")]
    public class Keyword
    {
        [Key] [DataMember] public int KeywordId { get; set; }

        [DataMember] public string Name { get; set; }

        public List<Item> Items { get; set; }
    }
}