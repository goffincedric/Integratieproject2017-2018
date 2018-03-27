using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Items;
using Domain.JSONConversion;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PB.DAL.EF;

namespace PB.DAL
{
    public class RecordRepo : IRecordRepo
    {
        private IntegratieDbContext ctx;

        public RecordRepo()
        {
            ctx = new IntegratieDbContext();
        }

        public RecordRepo(UnitOfWork uow)
        {
            ctx = uow.Context;
            Console.WriteLine("UOW MADE RECORD REPO");
        }

        public Record CreateRecord(Record record)
        {
            ctx.Records.Add(record);
            ctx.SaveChanges();
            return record;
        }

        public void DeleteRecord(long id)
        {
            ctx.Records.Remove(ReadRecord(id));
            ctx.SaveChanges();
        }

        public Record ReadRecord(long Tweet_Id)
        {
            return ctx.Records.FirstOrDefault(r => r.Tweet_Id == Tweet_Id);
        }

        public IEnumerable<Record> ReadRecords()
        {
            return ctx.Records.AsEnumerable();
        }

        public Mention ReadMention(string name)
        {
            return ctx.Mentions.FirstOrDefault(m => m.Name.ToLower().Equals(name.ToLower()));
        }

        public Hashtag ReadHashtag(string tag)
        {
            return ctx.Hashtags.FirstOrDefault(h => h.tag.ToLower().Equals(tag.ToLower()));
        }

        public Word ReadWord(string word)
        {
            return ctx.Words.FirstOrDefault(w => w.Text.ToLower().Equals(word.ToLower()));
        }

        public Url ReadUrl(string link)
        {
            return ctx.Urls.FirstOrDefault(u => u.Link.ToLower().Equals(link.ToLower()));
        }

        public void UpdateRecord(Record record)
        {
            ctx.Records.Attach(record);
            ctx.Entry(record).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }

        public int GetNumberofMentions(Record record)
        {
            return record.Mentions.Count;
        }

        public IEnumerable<Record> GetAllRecordsBefore(Record record, DateTime end) =>
          // Returnt een lijst van Records met vermelding van dezelfde politieker. Er kan een einddatum worden meegegeven. 
          ctx.Records.Where(x => x.Date < end)
            .Concat(ctx.Records.Where(x => x.RecordPerson.Number.Equals(record.RecordPerson.Number)));

        public List<Record> Seed(bool even)
        {
            Random random = new Random();
            var list = JsonConvert.DeserializeObject<List<JCLASS>>(File.ReadAllText(@"TestData\textgaindump.json")).ToList().Where(r => (even) ? r.Id % 2 == 0 : r.Id % 2 != 0);

            List<Mention> mentions;
            List<Word> words;
            List<Hashtag> hashtags;
            List<Url> urls;

            List<Record> recordsToAdd = new List<Record>();

            foreach (var el in list)
            {

                Record record = new Record()
                {
                    Tweet_Id = el.Id,
                    User_Id = el.User_Id,
                    Source = el.Source,
                    Date = el.Date,
                    Geo = el.Geo,
                    RecordPerson = new RecordPerson() { FirstName = el.Politician[0], LastName = el.Politician[1] },
                    Retweet = el.Retweet,
                    Sentiment = new Sentiment(el.Sentiment[0], el.Sentiment[1]),
                    ListUpdatet = DateTime.Now,

                };

                mentions = new List<Mention>();
                foreach (var m in el.Mentions)
                {
                    Mention mentionCheck = ReadMention(m);
                    if (mentionCheck != null)
                    {
                        mentions.Add(mentionCheck);

                    }
                    else
                    {
                        Mention mention = new Mention(m);
                        mentions.Add(mention);

                    }

                }

                record.Mentions = mentions;


                words = new List<Word>();
                foreach (var w in el.Words)
                {
                    Word wordCheck = ReadWord(w);
                    if (wordCheck != null)
                    {
                        words.Add(wordCheck);

                    }
                    else
                    {
                        Word word = new Word(w);
                        words.Add(word);

                    }

                }

                record.Words = words;

                hashtags = new List<Hashtag>();
                foreach (var h in el.Hashtags)
                {
                    Hashtag hashtagCheck = ReadHashtag(h);
                    if (hashtagCheck != null)
                    {
                        hashtags.Add(hashtagCheck);
                    }
                    else
                    {
                        Hashtag tag = new Hashtag(h);
                        hashtags.Add(tag);
                    }
                }

                record.Hashtags = hashtags;

                urls = new List<Url>();
                foreach (var u in el.URLs)
                {
                    Url urlCheck = ReadUrl(u);
                    if (urlCheck != null)
                    {
                        urls.Add(urlCheck);
                    }
                    else
                    {
                        Url url = new Url(u);
                        urls.Add(url);
                    }
                }

                record.URLs = urls;


                if (recordsToAdd.Find(r => r.Tweet_Id == record.Tweet_Id) == null)
                {
                    recordsToAdd.Add(record);
                    ctx.Records.Add(record);
                    ctx.SaveChanges();
                    ctx.CommitChanges();
                }



                //if (recordsToAdd.FirstOrDefault(r => r.Tweet_Id == record.Tweet_Id) != null)
                //{
                //  recordsToAdd[recordsToAdd.FindIndex(r => r.Tweet_Id == record.Tweet_Id)] = record;
                //}
                //else
                //{
                //  recordsToAdd.Add(record);
                //}
            }

            //ctx.Records.AddRange(recordsToAdd);
            //ctx.SaveChanges();

            return recordsToAdd;
        }
    }
}


