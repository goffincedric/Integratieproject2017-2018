using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PB.BL.Domain.Items
{
    [Table("tblRecord")]
    [DataContract]
    public class Record
    {   
        [DataMember]
        [Key] public long Tweet_Id { get; set; }

        [DataMember]
        public string Source { get; set; }
        [DataMember]
        public double? Longitude { get; set; }
        [DataMember]
        public double? Latitude { get; set; }
        [DataMember]
        public bool Retweet { get; set; }
        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public RecordProfile RecordProfile { get; set; }
        [DataMember]
        public Sentiment Sentiment { get; set; }

        [DataMember]
        public virtual List<Hashtag> Hashtags { get; set; }
        [DataMember]
        public virtual List<Mention> Mentions { get; set; }
        [DataMember]
        public virtual List<Url> URLs { get; set; }
        public List<Theme> Themes { get; set; }
        public List<Person> Persons { get; set; }
        [DataMember]
        public virtual List<Word> Words { get; set; }

        public override string ToString()
        {
            string text = Date + " - Persons: ";
            Persons.ForEach(p => text += p.Name + ", ");
            return text.Substring(0, text.Length - 2) + " (" + Tweet_Id + ")";
        }
    }
}