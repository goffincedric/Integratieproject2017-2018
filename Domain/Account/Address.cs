using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

    [Required]
    [ForeignKey("Username")]
    public T Profile { get; set; }

    public Address(Province province)
    {
      this.Province = province;
    }    
  }
}
