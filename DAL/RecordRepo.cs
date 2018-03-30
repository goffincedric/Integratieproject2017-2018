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
            return ctx.Records
                .Include("Mentions")
                .Include("RecordPerson")
                .Include("Words")
                .Include("Hashtags")
                .Include("URLs")
                .Include("Items")
                .AsEnumerable();
        }

        public Mention ReadMention(string name)
        {
            return ctx.Mentions.FirstOrDefault(m => m.Name.ToLower().Equals(name.ToLower()));
        }

        public Hashtag ReadHashtag(string tag)
        {
            return ctx.Hashtags.FirstOrDefault(h => h.HashTag.ToLower().Equals(tag.ToLower()));
        }

        public Word ReadWord(string word)
        {
            return ctx.Words.FirstOrDefault(w => w.Text.ToLower().Equals(word.ToLower()));
        }

        public Url ReadUrl(string link)
        {
            return ctx.Urls.FirstOrDefault(u => u.Link.ToLower().Equals(link.ToLower()));
        }

        public RecordPerson ReadRecordPerson(string firstName, string lastName)
        {
            return ctx.RecordPeople.FirstOrDefault(rp => rp.FirstName.Equals(firstName) && rp.LastName.Equals(lastName));
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
            .Concat(ctx.Records.Where(x => x.RecordPerson.Id.Equals(record.RecordPerson.Id)));

        public List<Record> Seed(bool even)
        {

            Random random = new Random();
            var list = JsonConvert.DeserializeObject<List<JCLASS>>(File.ReadAllText(@"TestData\textgaindump.json")).ToList().Where(r => (even) ? r.Id % 2 == 0 : r.Id % 2 != 0);

            List<Mention> allMentions = ctx.Mentions.ToList();
            List<Word> allWords = ctx.Words.ToList();
            List<Hashtag> allHashtags = ctx.Hashtags.ToList();
            List<Url> allUrls = ctx.Urls.ToList();
            List<RecordPerson> recordPeople = new List<RecordPerson>();


            List<Record> oldRecords = ReadRecords().ToList();
            List<Record> newRecords = new List<Record>();

            foreach (var el in list)
            {
                Record record = new Record()
                {
                    Tweet_Id = el.Id,
                    User_Id = el.User_Id,
                    Source = el.Source,
                    Date = el.Date,
                    Geo = el.Geo,
                    Retweet = el.Retweet,
                    Sentiment = new Sentiment(el.Sentiment[0], el.Sentiment[1]),
                    ListUpdatet = DateTime.Now,
                    Mentions = new List<Mention>(),
                    Words = new List<Word>(),
                    Hashtags = new List<Hashtag>(),
                    URLs = new List<Url>()
                };


                RecordPerson recordPerson = recordPeople.FirstOrDefault(rp => rp.FirstName.Equals(el.Politician[0]) && rp.LastName.Equals(el.Politician[1]));
                if (recordPerson != null)
                {
                    record.RecordPerson = recordPerson;
                }
                else
                {
                    recordPerson = new RecordPerson()
                    {
                        FirstName = el.Politician[0],
                        LastName = el.Politician[1]
                    };
                    recordPeople.Add(recordPerson);
                    record.RecordPerson = recordPerson;
                }


                foreach (var m in el.Mentions)
                {
                    Mention mentionCheck = allMentions.Find(me => me.Name.Equals(m));
                    if (mentionCheck != null)
                    {
                        record.Mentions.Add(mentionCheck);
                    }
                    else
                    {
                        Mention mention = new Mention(m);
                        record.Mentions.Add(mention);
                        allMentions.Add(mention);
                    }
                }


                foreach (var w in el.Words)
                {
                    Word wordCheck = allWords.Find(wo => wo.Text.Equals(w));
                    if (wordCheck != null)
                    {
                        record.Words.Add(wordCheck);

                    }
                    else
                    {
                        Word word = new Word(w);
                        record.Words.Add(word);
                        allWords.Add(word);
                    }
                }


                foreach (var h in el.Hashtags)
                {
                    Hashtag hashtagCheck = allHashtags.Find(ha => ha.HashTag.Equals(h));
                    if (hashtagCheck != null)
                    {
                        record.Hashtags.Add(hashtagCheck);
                    }
                    else
                    {
                        Hashtag tag = new Hashtag(h);
                        record.Hashtags.Add(tag);
                        allHashtags.Add(tag);
                    }
                }


                foreach (var u in el.URLs)
                {
                    Url urlCheck = allUrls.Find(url => url.Link.Equals(u));
                    if (urlCheck != null)
                    {
                        record.URLs.Add(urlCheck);
                    }
                    else
                    {
                        Url url = new Url(u);
                        record.URLs.Add(url);
                        allUrls.Add(url);
                    }
                }


                if (oldRecords.FirstOrDefault(r => r.Tweet_Id == record.Tweet_Id) == null)
                {
                    if (newRecords.FirstOrDefault(r => r.Tweet_Id == record.Tweet_Id) != null)
                    {
                        newRecords[newRecords.FindIndex(r => r.Tweet_Id == record.Tweet_Id)] = record;
                    }
                    else
                    {
                        newRecords.Add(record);
                    }
                }
            }

            ctx.Records.AddRange(newRecords);
            ctx.SaveChanges();
            ctx.CommitChanges();

            return newRecords;
        }
    }
}


