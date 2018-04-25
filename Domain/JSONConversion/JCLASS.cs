using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using System;
using System.Collections.Generic;

namespace PB.BL.Domain.JSONConversion
{
    public class JClass
    {
        public RecordProfile Profile { get; set; }
        public List<String> Words { get; set; }
        public List<double> Sentiment { get; set; }
        public string Source { get; set; }
        public List<String> Hashtags { get; set; }
        public List<String> Mentions { get; set; }
        public List<String> URLs { get; set; }
        public List<string> Themes { get; set; }
        public List<string> Persons { get; set; }
        public DateTime Date { get; set; }
        public List<double?> Geo { get; set; }
        public long Id { get; set; }
        public bool Retweet { get; set; }

        public List<Subplatform> Subplatforms { get; set; }
        
        public JClass(RecordProfile profile, List<string> words, List<double> sentiment, string source, List<string> hashtags, List<string> mentions, List<string> uRLs, List<string> themes, List<string> persons, DateTime date, dynamic geo, long id, bool retweet)
        {
            Profile = profile;
            Words = words;
            Sentiment = sentiment;
            Source = source;
            Hashtags = hashtags;
            Mentions = mentions;
            URLs = uRLs;
            Themes = themes;
            Persons = persons;
            Date = date;
            Geo = (geo is bool) ? null : geo.ToObject<List<double?>>();
            Id = id;
            Retweet = retweet;

            //Console.WriteLine("====");
            //Console.WriteLine(sentiment is null);
            //Console.WriteLine(sentiment[0]);
            //Console.WriteLine(sentiment[1]);

            Subplatforms = new List<Subplatform>();
        }
    }
}
