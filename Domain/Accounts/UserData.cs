using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Account
{
<<<<<<< HEAD:Domain/Account/UserData.cs
  [Table("tblUserData")]
  public class UserData
  {

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Telephone { get; set; }
    public string Street { get; set; }
    public int? PostalCode { get; set; }
    public string City { get; set; }
    public DateTime BirthDate { get; set; }
    public Province Province { get; set; }
    public Gender Gender { get; set; }
=======
    [Table("tblUserData")]
    public class UserData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Telephone { get; set; }
        public string StreetNbr { get; set; }
        public int? PostalCode { get; set; }
        public string City { get; set; }
        public DateTime BirthDate { get; set; }
        [Required]
        public Province Province { get; set; }
        public Gender Gender { get; set; }
>>>>>>> master:Domain/Accounts/UserData.cs

    [Key]
    public string Username { get; set; }

    [Required]
    [ForeignKey("Username")]
    public Profile Profile { get; set; }
  }
}
