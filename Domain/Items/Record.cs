using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Items
{
  [Table("tblRecord")]
  public class Record
  {

    public string Source { get; set; }
    [Key]
    public long Id { get; set; }
    public string User_Id { get; set; }
    public List<Mention> Mentions { get; set; }
    //public string PoliticianFirstName { get; set; }
    //public string PoliticianLastName { get; set; }
    public DateTime Date;
    public string Geo { get; set; }
    public List<Politician> Politician { get; set; }
    public bool Retweet { get; set; }
    //public DateTime BirthDay { get; set; }
    public List<Words> Words { get; set; }
    //public double Polarity { get; set; }
    //public double Objectivity { get; set; }
    public Sentiment Sentiment { get; set; }
    public List<Hashtag> Hashtags { get; set; }
    public List<Url> URLs { get; set; }
    public DateTime ListUpdatet { get; set; }

    public override string ToString()
    {
      return "User_Id: " + User_Id + "\nTweetId: " + Id + "\nNaam: " + Politician[0] + " " + Politician[1];
    }
  }
}
