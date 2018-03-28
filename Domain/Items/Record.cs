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
        public string Source { get; set; }
        public string User_Id { get; set; }
        public List<Mention> Mentions { get; set; }
        public DateTime Date { get; set; }
        public string Geo { get; set; }
        public int RecordPersonId { get; set; }
        public RecordPerson RecordPerson { get; set; }
        public bool Retweet { get; set; }
        public List<Word> Words { get; set; }
        public Sentiment Sentiment { get; set; }
        public List<Hashtag> Hashtags { get; set; }
        public List<Url> URLs { get; set; }
        public DateTime ListUpdatet { get; set; }
        
        public List<Item> Items { get; set; }

        public override string ToString()
        {
            return "User_Id: " + User_Id + "\nTweetId: " + Tweet_Id;
        }
    }
}
