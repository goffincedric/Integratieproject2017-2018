using PB.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Items
{
    [Table("tblRecord")]
    public class Record
    {
        [Key]
        public long Tweet_Id { get; set; }
        public RecordProfile RecordProfile { get; set; }
        public List<Word> Words { get; set; }
        public Sentiment Sentiment { get; set; }
        public string Source { get; set; }
        public List<Hashtag> Hashtags { get; set; }
        public List<Mention> Mentions { get; set; }
        public List<Url> URLs { get; set; }
        public List<Item> Themes { get; set; }
        public List<Item> Persons { get; set; }
        public DateTime Date { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public bool Retweet { get; set; }
        public DateTime ListUpdatet { get; set; }

        public override string ToString()
        {
            string text = "\nTweetId: " + Tweet_Id + " Date: " + Date + " Persons: ";
            Persons.ForEach(p => text += " " + p.ToString() + ", ");
            return text.Substring(0, text.Length - 3);
        }
    }
}
