using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
      
      JsonConvert.DeserializeObject<List<Record>>(File.ReadAllText(@"TestData\textgaindump.json")).ForEach(r => ctx.Records.Add(r));
      ctx.SaveChanges();
    }
  }
}
