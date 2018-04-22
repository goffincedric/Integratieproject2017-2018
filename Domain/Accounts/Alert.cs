using PB.BL.Domain.Items;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Account
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
        public bool IsRead { get; set; }
        [Required]
        public DateTime TimeStamp { get; set; }
        public string UserId { get; set; }
        public int ItemId { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public Profile Profile { get; set; }
        [Required]
        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Alert)) return false;
            Alert alert = (Alert)obj;

            if (!alert.Text.ToLower().Equals(Text.ToLower())) return false;
            if (!alert.Description.ToLower().Equals(Description.ToLower())) return false;
            if (!alert.UserId.ToLower().Equals(UserId.ToLower())) return false;

            if (!alert.TimeStamp.Date.Equals(TimeStamp.Date)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Description + "; " + Text + ";";
        }
    }
}
