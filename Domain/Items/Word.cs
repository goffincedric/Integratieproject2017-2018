using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Items
{
    [DataContract]
    [Table("tblWords")]
    public class Word
    {
        [Key]
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public String Text { get; set; }

        public List<Record> Records { get; set; }

        public Word()
        {

        }

        public Word(string text)
        {
            Text = text;
        }
    }
}
