﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PB.BL.Domain.Items
{
    [DataContract]
    [Table("tblTheme")]
    public class Theme : Item
    {
        [DataMember] public string Description { get; set; }


        public virtual List<Record> Records { get; set; }

        public virtual List<Person> Persons { get; set; }

        public virtual List<Organisation> Organisations { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Theme theme &&
                   Name.ToLower().Equals(theme.Name.ToLower());
        }

        public override int GetHashCode()
        {
            var hashCode = 56787390;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }

        public override string ToString()
        {
            return Name + ": " + Description + " (Id: " + ItemId + ") - Aantal records: ";
        }
    }
}