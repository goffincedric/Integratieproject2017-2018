using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Items
{
  public class Record
  {
    public string Source { get; set; }
    public int TweetId { get; set; }
    public string UserId { get; set; }
    public string PoliticianFirstName { get; set; }
    public string PoliticianLastName { get; set; }
    public string Geo { get; set; }
    public List<string> Mentions { get; set; }
    public bool Retweet { get; set; }

  }
}
