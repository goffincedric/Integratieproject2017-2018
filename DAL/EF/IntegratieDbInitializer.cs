using System.Data.Entity;
using PB.DAL.Migrations;

namespace PB.DAL.EF
{
    internal class IntegratieDbInitializer : MigrateDatabaseToLatestVersion<IntegratieDbContext, Configuration>
    {
    }
}