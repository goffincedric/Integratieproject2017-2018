using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Items
{
<<<<<<< HEAD:Domain/Items/Politician.cs
  [Table("tblPolitician")]
  public class Politician
=======
  [Table("tblPoliticianRecord")]
  public class RecordPerson
>>>>>>> master:Domain/Items/RecordPerson.cs
  {
    [Key] public int Number { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public RecordPerson(string firstName, string lastName)
    {
      FirstName = firstName;
      LastName = lastName;
    }
  }
}
