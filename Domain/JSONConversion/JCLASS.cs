using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.JSONConversion
{
  public class JCLASS
  {
    public List<String> Hashtags { get; set; }

    public List<String> Words { get; set; }

    public DateTime Date;

    public List<String> Politician { get; set; }

    public string Geo { get; set; }
    public long Id { get; set; }

    public string User_Id { get; set; }

    public List<double> Sentiment { get; set; }

    public bool Retweet { get; set; }

    public string Source { get; set; }

    public List<String> URLs { get; set; }

    public List<String> Mentions { get; set; }
  }
}
