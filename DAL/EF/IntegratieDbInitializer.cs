using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PB.BL.Domain.Items;

namespace PB.DAL.EF
{
  internal class IntegratieDbInitializer : MigrateDatabaseToLatestVersion<IntegratieDbContext,DAL.Migrations.Configuration>
  {
    public IntegratieDbInitializer()
    {
    }
    
   
    }
  }

