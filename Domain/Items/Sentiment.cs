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
    public double Prohability { get; set; }

    public Sentiment(double objectiviteit, double probabiliteit)
    {
      Objectivity = objectiviteit;
      Prohability = probabiliteit;
    }

    public Sentiment()
    {
    }
  }
}
