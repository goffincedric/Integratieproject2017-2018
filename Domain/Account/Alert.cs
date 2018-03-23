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

    public override string ToString()
    {
      return Description + "; " + Text + ";";
    }
  }
}
