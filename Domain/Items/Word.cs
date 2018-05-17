using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PB.BL.Domain.Items
{
    [DataContract]
    [Table("tblWords")]
    public class Word
    {
        public Word()
        {
        }

        public Word(string text)
        {
            Text = text;
        }

        [Key] [DataMember] public int Id { get; set; }

        [DataMember] public string Text { get; set; }

        public List<Record> Records { get; set; }
    }
}