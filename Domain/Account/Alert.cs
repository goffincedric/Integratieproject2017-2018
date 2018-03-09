using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Account
{
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

    [Required]
    [ForeignKey("Username")]
    public T Profile { get; set; }

    public Alert(int alertId, string text, bool isFlaggedImportant, bool isRead, DateTime timeStamp)
    {
      AlertId = alertId;
      Text = text;
      IsFlaggedImportant = isFlaggedImportant;
      IsRead = isRead;
      TimeStamp = timeStamp;
    }

    public Alert(int alertId, string text, string description = null, bool isFlaggedImportant, bool isRead, DateTime timeStamp)
    {
      AlertId = alertId;
      Text = text;
      Description = description;
      IsFlaggedImportant = isFlaggedImportant;
      IsRead = isRead;
      TimeStamp = timeStamp;
    }
  }
}
