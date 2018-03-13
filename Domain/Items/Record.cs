using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Items
{
  [Table("tblRecord")]
  public class Record
  {

    public string Source { get; set; }
    [Key]
    public long Id { get; set; }
    public string User_Id { get; set; }
    public List<string> Mentions { get; set; }
    //public string PoliticianFirstName { get; set; }
    //public string PoliticianLastName { get; set; }
    public DateTime Date;
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

    public override string ToString()
    {
      return "User_Id: " + User_Id + "\nTweetId: " + Id + "\nNaam: " + Politician[0] + " " + Politician[1];
    }
  }
}
