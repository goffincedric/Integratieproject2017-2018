using PB.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Items;

namespace PB.DAL
{
  public interface IRecordRepo
  {
    IEnumerable<Record> ReadRecords();
    Record CreateRecord(Record record);
    Record ReadRecord(long id);
    void UpdateRecord(Record record);
    void DeleteRecord(long id);

    void Seed();
  }
}
