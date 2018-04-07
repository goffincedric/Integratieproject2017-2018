using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI_MVC.Models
{
  public class MasterProfile
  {
   public ExternalLoginListViewModel external { get; set; }
    public PB.BL.Domain.Account.Profile profile { get; set; }
  }
}