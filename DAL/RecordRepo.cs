﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Items;
using Domain.JSONConversion;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PB.BL.Domain.Items;
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

    public Record CreateRecord(Record record)
    {
      ctx.Records.Add(record);
      return record;
    }

    public void DeleteRecord(long id)
    {
      ctx.Records.Remove(ReadRecord(id));
    }

    public Record ReadRecord(long id)
    {
      return ctx.Records.FirstOrDefault(r => r.Id == id);
    }

    public IEnumerable<Record> ReadRecords()
    {
      return ctx.Records.AsEnumerable();
    }

    public void UpdateRecord(Record record)
    {
      //MemoryRepo, alle objecten worden automatisch geüpdatet in het geheugen
    }

    public void Seed()
    {

      var list = JsonConvert.DeserializeObject<List<JCLASS>>(File.ReadAllText(@"TestData\textgaindump.json"));

      List<Mention> mentions;
      List<Words> words;
      List<Politician> politicians;
      List<Hashtag> hashtags;
      List<Url> urls;

      foreach (var el in list)
      {

        mentions = new List<Mention>();

        foreach (var m in el.Mentions)
        {
          mentions.Add(new Mention(m));
        }

        words = new List<Words>();

        foreach (var w in el.Words)
        {

          words.Add(new Words(w));
        }

        politicians = new List<Politician>();
        foreach (var p in el.Politician)
        {
          politicians.Add(new Politician(p));
        }

        hashtags = new List<Hashtag>();
        foreach (var h in el.Hashtags)
        {
          hashtags.Add(new Hashtag(h));
        }

        urls = new List<Url>();
        foreach (var u in el.URLs)
        {
          urls.Add(new Url(u));
        }

        Record record = new Record()
        {
          Id = el.Id,
          User_Id = el.User_Id,
          Mentions = mentions,
          Source = el.Source,
          Date = el.Date,
          Geo = el.Geo,
          Politician = politicians,
          Retweet = el.Retweet,
          Sentiment = new Sentiment(el.Sentiment[0], el.Sentiment[1]),
          Hashtags = hashtags,
          URLs = urls,
          ListUpdatet = DateTime.Now,
          Words = words

        };
        ctx.Records.Add(record);

      }

      ctx.SaveChanges();


    }
  }
}
