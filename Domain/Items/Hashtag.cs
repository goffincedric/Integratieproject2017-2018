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
    [Table("tblHashtag")]
    public class Hashtag
    {
        [Key]
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public String HashTag { get; set; }

        public List<Record> Records { get; set; }

        public Hashtag()
        {

        }

        public Hashtag(string hashTag)
        {
            HashTag = hashTag;
        }
    }
}
