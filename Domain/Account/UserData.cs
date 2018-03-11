﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Account
{
  [Table("tblUserData")]
  public class UserData
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Telephone { get; set; }
    public Address Address { get; set; }
    public Gender Gender { get; set; }

    [Required]
    [ForeignKey("Username")]
    public Profile Profile { get; set; }
  }
}
