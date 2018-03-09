using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Account
{
  public class Alert
  {
    public string Text { get; set; }
    public string Description { get; set; }
    public bool IsFlaggedImportant { get; set; }
    public bool IsRead { get; set; }
    public DateTime TimeStamp { get; set; }
  }
}
