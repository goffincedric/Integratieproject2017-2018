using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Items
{
  public class Record
  {
    public string Source { get; set; }
    [Key]
    public int Id { get; set; }
    public string User_Id { get; set; }
    public List<string> Mentions { get; set; }
    //public string PoliticianFirstName { get; set; }
    //public string PoliticianLastName { get; set; }
    public string Geo { get; set; }
    public List<string> Politician { get; set; }
    public bool Retweet { get; set; }
    //public DateTime BirthDay { get; set; }
    public List<string> Words { get; set; }
    //public double Polarity { get; set; }
    //public double Objectivity { get; set; }
    public List<double> Sentiment { get; set; }
    public List<string> Hashtags { get; set; }
    public List<string> URLs { get; set; }
    public DateTime ListUpdatet { get; set; }

    public Record(string source, int id, string user_Id, List<string> mentions, string geo, List<string> politician, bool retweet, List<string> words, List<double> sentiment, List<string> hashtags, List<string> uRLs)
    {
      Source = source;
      Id = id;
      User_Id = user_Id;
      Mentions = mentions;
      Geo = geo;
      Politician = politician;
      Retweet = retweet;
      Words = words;
      Sentiment = sentiment;
      Hashtags = hashtags;
      URLs = uRLs;
    }
  }
}
