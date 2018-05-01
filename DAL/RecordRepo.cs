using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PB.BL.Domain.Items;
using PB.BL.Domain.JSONConversion;
using PB.DAL.EF;

namespace PB.DAL
{
    public class RecordRepo : IRecordRepo
    {
        private readonly IntegratieDbContext ctx;

        public RecordRepo()
        {
            ctx = new IntegratieDbContext();
        }

        public RecordRepo(UnitOfWork uow)
        {
            ctx = uow.Context;
            //Console.WriteLine("UOW MADE RECORD REPO");
        }

        public Record CreateRecord(Record record)
        {
            ctx.Records.Add(record);
            ctx.SaveChanges();
            return record;
        }
        
        public IEnumerable<Record> CreateRecords(IEnumerable<Record> records)
        {
            ctx.Records.AddRange(records);
            ctx.SaveChanges();
            return records;
        }

        public void DeleteRecord(long id)
        {
            ctx.Records.Remove(ReadRecord(id));
            ctx.SaveChanges();
        }

        public void DeleteRecords(IEnumerable<Record> records)
        {
            ctx.Records.RemoveRange(records);
            ctx.SaveChanges();
        }

        public Record ReadRecord(long Tweet_Id)
        {
            return ctx.Records.FirstOrDefault(r => r.Tweet_Id == Tweet_Id);
        }

        public IEnumerable<Record> ReadRecords()
        {
            return ctx.Records
                .Include(r => r.Mentions)
                .Include(r => r.Persons)
                .Include(r => r.Words)
                .Include(r => r.Hashtags)
                .Include(r => r.URLs)
                .Include(r => r.Themes)
                .AsEnumerable();
        }

        public IEnumerable<Mention> ReadMentions()
        {
            return ctx.Mentions
                .Include(m => m.Records)
                .AsEnumerable();
        }

        public IEnumerable<Word> ReadWords()
        {
            return ctx.Words.AsEnumerable();
        }

        public IEnumerable<Hashtag> ReadHashTags()
        {
            return ctx.Hashtags.AsEnumerable();
        }

        public IEnumerable<Url> ReadUrls()
        {
            return ctx.Urls.AsEnumerable();
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

        public void UpdateRecord(Record record)
        {
            ctx.Records.Attach(record);
            ctx.Entry(record).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }

        public IEnumerable<Record> GetAllRecordsBefore(Person person, DateTime end) =>
          // Returnt een lijst van Records met vermelding van dezelfde politieker. Er kan een einddatum worden meegegeven. 
          ctx.Records.Where(x => x.Date < end)
            .Concat(ctx.Records.Where(r => r.Persons.Contains(person)));
    }
}


