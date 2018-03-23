using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PB.BL.Domain.Account;

namespace Domain.Account
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

    public string Username { get; set; }

    [Required]
    [ForeignKey("Username")]
    public Profile Profile { get; set; }

    public override bool Equals(object obj)
    {
      if (!(obj is Alert)) return false;
      Alert alert = (Alert)obj;

      if (!alert.Text.ToLower().Equals(this.Text.ToLower())) return false;
      if (!alert.Description.ToLower().Equals(this.Description.ToLower())) return false;
      if (!alert.Username.ToLower().Equals(this.Username.ToLower())) return false;
      
      if (!alert.TimeStamp.Date.Equals(this.TimeStamp.Date)) return false;
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
