using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Items
{
  
  public class Sentiment
  {
    public double Objectiviteit { get; set; }
    public double Probabiliteit { get; set; }

    public Sentiment(double objectiviteit, double probabiliteit)
    {
      Objectiviteit = objectiviteit;
      Probabiliteit = probabiliteit;
    }

    public Sentiment()
    {
    }
  }
}
