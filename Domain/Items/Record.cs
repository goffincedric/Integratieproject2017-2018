using PB.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Items
{
    [Table("tblRecord")]
    public class Record
    {
        [Key]
        public long Tweet_Id { get; set; }
        public string Source { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public bool Retweet { get; set; }
        public DateTime Date { get; set; }

        public RecordProfile RecordProfile { get; set; }
        public Sentiment Sentiment { get; set; }

        public virtual List<Hashtag> Hashtags { get; set; }
        public virtual List<Mention> Mentions { get; set; }

        public virtual List<Url> URLs { get; set; }
        public List<Theme> Themes { get; set; }
        public List<Person> Persons { get; set; }
        public virtual List<Word> Words { get; set; }

        public override string ToString()
        {
            string text = Date.ToString() + " - Persons: ";
            Persons.ForEach(p => text += p.Name + ", ");
            return text.Substring(0, text.Length - 2) + " (" + Tweet_Id + ")";
        }
    }
}
