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
    public double Objectivity { get; set; }
    public double Polarity { get; set; }

    public Sentiment()
    {
    }

    public Sentiment(double objectiviteit, double polarity)
    {
      Objectivity = objectiviteit;
      Polarity = polarity;
    }
  }
}
