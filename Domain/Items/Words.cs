using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Items
{
  [Table("tblWordsRecord")]
  public class Words
  {
    [Key] public int Number { get; set; }
    public String Word { get; set; }

    public Words(string word)
    {
      Word = word;
    }
  }
}
