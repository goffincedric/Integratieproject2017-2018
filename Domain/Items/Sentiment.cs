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
