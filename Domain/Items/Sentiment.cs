namespace PB.BL.Domain.Items
{
    public class Sentiment
    {
        public Sentiment()
        {
        }

        public Sentiment(double objectiviteit, double polarity)
        {
            Objectivity = objectiviteit;
            Polarity = polarity;
        }

        public double Objectivity { get; set; }
        public double Polarity { get; set; }
    }
}