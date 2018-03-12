using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PB.BL.Domain.Items;

namespace PB.DAL
{
  public class RecordRepoHC : IRecordRepoHC
  {
    private List<Record> recordsRepo = new List<Record>();

    public Record CreateRecord(Record record)
    {
      recordsRepo.Add(record);
      return record;
    }

    public void DeleteRecord(long id)
    {
      recordsRepo.Remove(ReadRecord(id));
    }

    public Record ReadRecord(long id)
    {
      return recordsRepo.FirstOrDefault(r => r.Id == id);
    }

    public IEnumerable<Record> ReadRecords()
    {
      return recordsRepo.AsEnumerable();
    }

    public void UpdateRecord(Record record)
    {
      //MemoryRepo, alle objecten worden automatisch geüpdatet in het geheugen
    }

    public void Seed()
    {
      recordsRepo = JsonConvert.DeserializeObject<List<Record>>(File.ReadAllText(@"TestData\textgaindump.json"));
    }
  }
}
