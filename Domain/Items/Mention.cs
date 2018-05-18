using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PB.BL.Domain.Items
{
    [DataContract]
    [Table("tblMention")]
    public class Mention
    {
        public Mention()
        {
        }

        public Mention(string name)
        {
            Name = name;
        }

        [Key] [DataMember] public int Id { get; set; }

        [DataMember] public string Name { get; set; }

        public List<Record> Records { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}