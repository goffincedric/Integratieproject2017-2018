using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Items
{
    [DataContract]
    [Table("tblMention")]
    public class Mention
    {
        [Key]
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }

        public List<Record> Records { get; set; }

        public Mention()
        {

        }
        public Mention(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
