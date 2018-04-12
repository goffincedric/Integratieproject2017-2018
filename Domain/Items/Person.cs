using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Items;

namespace PB.BL.Domain.Items
{
    [Table("tblPerson")]
    public class Person : Item
    {
        public string SocialMediaLink { get; set; }
        public string IconURL { get; set; }

        public Function Function { get; set; }
        public Organisation Organisation { get; set; }
        public List<Record> Records { get; set; }

        public override string ToString()
        {
            return Name + " (Id: " + (ItemId) + ") - Aantal records: " + Records.Count;
        }
    }
}
