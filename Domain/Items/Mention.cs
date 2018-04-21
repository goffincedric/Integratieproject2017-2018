﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Items
{
    [Table("tblMention")]
    public class Mention
    {
        [Key]
        public int Id { get; set; }
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
