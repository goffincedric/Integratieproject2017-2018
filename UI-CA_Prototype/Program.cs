using PB.BL;
using PB.BL.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI_CA_Prototype
{
  class Program
  {
    static void Main(string[] args)
    {
      AccountManager mgr = seed();

      List<Alert> alerts = mgr.generateAlerts(mgr.GetProfile("goffincedric"));
    }

    private static AccountManager seed()
    {
      AccountManager mgr = new AccountManager();

      mgr.AddProfile("test@schlack.tld", "goffincedric", "test123");
      
      return mgr;
    }
  }
}
