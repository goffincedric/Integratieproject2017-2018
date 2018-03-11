using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.DAL.EF
{
  public class IntegratieDbContext: System.Data.Entity.DbContext
  {
    public IntegratieDbContext(): base("name=CONNECTIONSTRING")
    {
      System.Data.Entity.Database.SetInitializer<IntegratieDbContext>(new IntegratieDbInitializer());
    }
  }
}
