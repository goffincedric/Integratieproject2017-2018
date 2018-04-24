using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Items
{
    [DataContract]
    [Table("tblUrl")]
    public class Url
    {
        [Key]
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Link { get; set; }

        public List<Record> Records { get; set; }

        public Url()
        {

        }

        public Url(string link)
        {
            Link = link;
        }
    }
}
