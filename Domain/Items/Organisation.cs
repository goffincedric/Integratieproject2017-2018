using Domain.Items;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Items
{
    [Table("tblOrganisation")]
    public class Organisation : Item
    {
        public string Description { get; set; }
        public string SocialMediaLink { get; set; }
        public string IconURL { get; set; }

        public List<Person> People { get; set; }
        public virtual List<Record> Records { get; set; }
    }
}
