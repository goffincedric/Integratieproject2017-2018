using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace PB.DAL.EF
{
  public class IntegratieDbInitializer : DropCreateDatabaseAlways<IntegratieDbContext>
  {
    public IntegratieDbInitializer()
    {
    }

    protected override void Seed(IntegratieDbContext context)
    {
      base.Seed(context);
    }
  }
}
