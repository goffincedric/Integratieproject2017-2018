using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Items;

namespace PB.BL.Domain.Items
{
    [Table("tblTheme")]
    public class Theme : Item
    {
        public string Description { get; set; }

        public virtual List<Record> Records { get; set; }

        public override string ToString()
        {
            return Name + " (Id: " + (ItemId) + ") - Aantal records: " + Records.Count;
        }
    }
}
