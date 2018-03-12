using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PB.BL.Domain.Items;

namespace PB.DAL.EF
{
  public class IntegratieDbInitializer : DropCreateDatabaseAlways<IntegratieDbContext>
  {
    public IntegratieDbInitializer()
    {
    }

    protected override void Seed(IntegratieDbContext context)
    {

      Record record1 = new Record()
      {
        Hashtags = new List<string>(),
        Words = new List<string>() {
          "annouri",
          "kasper goethals",
          "arabië",
          "imade",
          "iran"
        },
        Date = DateTime.Parse("2017-09-11 04:53:38"),
        Politician = new List<string>() {
          "Imade",
          "Annouri"
        },
        Geo = "N/A",
        Id = 907104827896987600,
        User_Id = "N/A",
        Sentiment = new List<double>() {
          0,
          0
        },
        Retweet = true,
        Source = "twitter",
        URLs = new List<string>(){
          "http://pltwps.it/_JY894kJ"
        },
        Mentions = new List<string>()
      };

      Record record2 = new Record()
      {
        Hashtags = new List<string>()
        {
          "Firsts,"
        },
        Words = new List<string>()
        {
          "annouri",
          "imade",
          "reeks",
          "time"
        },
        Date = DateTime.Parse("2017-09-07 22:52:35"),
        Politician = new List<string>()
        {
          "Imade",
          "Annouri"
        },
        Geo = "N/A",
        Id = 905926801804980200,
        User_Id = "N/A",
        Sentiment = new List<double>()
        {
          0.7,
          1
        },
        Retweet = false,
        Source = "twitter",
        URLs = new List<string>()
        {
          "https://twitter.com/TIME/status/905785286092877824",
          "http://pltwps.it/_xV6mWwE"
        },
        Mentions = new List<string>()

      };
    }
  }
}
