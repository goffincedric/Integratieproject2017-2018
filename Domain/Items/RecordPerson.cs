using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Items
{

  [Table("tblPoliticianRecord")]
  public class RecordPerson

  {
    [Key]
    public int Number { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public override string ToString()
    {
      return FirstName + " " + LastName;
    }

    public override bool Equals(object obj)
    {
      if (!(obj is RecordPerson)) return false;
      RecordPerson rp = (RecordPerson)obj;
      if (rp.FirstName.ToLower().Equals(this.FirstName.ToLower()) && rp.LastName.ToLower().Equals(this.LastName.ToLower())) return true;
      return false;
    }

    public override int GetHashCode()
    {
      var hashCode = 1938039292;
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FirstName);
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LastName);
      return hashCode;
    }
  }
}
