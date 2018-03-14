using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public string Username { get; set; }

    [Required]
    [ForeignKey("Username")]
    public Profile Profile { get; set; }
    
    }

}
