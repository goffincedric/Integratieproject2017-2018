using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Account
{
  public class Address
  {
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string City { get; set; }
    public Province Province { get; set; }
    public int? PostalCode { get; set; }

    public Address(Province province)
    {
      this.Province = province;
    }    
  }
}
