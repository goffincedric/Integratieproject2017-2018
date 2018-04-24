using System.Data.Entity;

namespace PB.DAL.EF
{
    internal class IntegratieDbInitializer : MigrateDatabaseToLatestVersion<IntegratieDbContext, DAL.Migrations.Configuration>
    {
        public IntegratieDbInitializer()
        {

        }
    }
}

