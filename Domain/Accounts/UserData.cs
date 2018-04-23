using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Accounts
{
    [Table("tblUserData")]
  public class UserData
  {
    [Key]
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Telephone { get; set; }
    public string Street { get; set; }
    public int? PostalCode { get; set; }
    public string City { get; set; }
    public DateTime BirthDate { get; set; }

    public Province Province { get; set; }
    public Gender Gender { get; set; }

    [Required]
    public Profile Profile { get; set; }
  }
}
