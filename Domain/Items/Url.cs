using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PB.BL.Domain.Items
{
    [DataContract]
    [Table("tblUrl")]
    public class Url
    {
        public Url()
        {
        }

        public Url(string link)
        {
            Link = link;
        }

        [Key] [DataMember] public int Id { get; set; }

        [DataMember] public string Link { get; set; }

        public List<Record> Records { get; set; }
    }
}