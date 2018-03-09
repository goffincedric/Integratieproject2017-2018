using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Account
{
  public class UserData
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Telephone { get; set; }
    public Address Address { get; set; }
    public Gender Gender { get; set; }
  }
}
