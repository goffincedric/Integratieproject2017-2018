using PB.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Accounts
{
    [Table("tblAlert")]
    public class Alert
    {
        [Key]
        public int AlertId { get; set; }
        [Required]
        public string Text { get; set; }
        public string Description { get; set; }
        public bool IsFlaggedImportant { get; set; }
        [Required]
        public List<ProfileAlert> ProfileAlerts { get; set; }
        public int ItemId { get; set; }

        [Required]
        public Item Item { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Alert)) return false;
            Alert alert = (Alert)obj;

            if (!alert.Text.ToLower().Equals(Text.ToLower())) return false;
            if (!alert.Description.ToLower().Equals(Description.ToLower())) return false;
            if (IsFlaggedImportant != IsFlaggedImportant) return false;
            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = -1128482448;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Text);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + IsFlaggedImportant.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return Description + "; " + Text + ";";
        }
    }
}
