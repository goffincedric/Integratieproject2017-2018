using PB.BL.Domain.Items;
using PB.BL.Domain.JSONConversion;
using System.Collections.Generic;

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
        IEnumerable<Record> CreateRecords(IEnumerable<Record> records);
        Record ReadRecord(long id);
        void UpdateRecord(Record record);
        void DeleteRecord(long id);
        void DeleteRecords(IEnumerable<Record> records);

        List<JClass> Seed(bool even);
    }
}
