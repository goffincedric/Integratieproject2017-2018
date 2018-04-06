using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Items;
using Domain.JSONConversion;

namespace PB.DAL
{
    public interface IRecordRepo
    {
        IEnumerable<Record> ReadRecords();
        IEnumerable<Mention> ReadMentions();
        IEnumerable<Word> ReadWords();
        IEnumerable<Hashtag> ReadHashTags();
        IEnumerable<Url> ReadUrls();

        Record CreateRecord(Record record);
        List<Record> CreateRecords(List<Record> records);
        Record ReadRecord(long id);
        void UpdateRecord(Record record);
        void DeleteRecord(long id);

        List<JClass> Seed(bool even);
    }
}
