using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Items
{
  [Table("tblWords")]
  public class Word
  {
    [Key] public int Id { get; set; }
    public String Text { get; set; }

    public List<Record> records { get; set; }

    public Word()
    {

    }

    public Word(string text)
    {
      Text = text;
    }
  }
}
