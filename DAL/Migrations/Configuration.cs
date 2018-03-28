namespace PB.DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PB.DAL.EF.IntegratieDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "PB.DAL.EF.IntegratieDbContext";
        }

        protected override void Seed(PB.DAL.EF.IntegratieDbContext context)
        {
      //  This method will be called after migrating to the latest version.

      //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
      //  to avoid creating duplicate seed data.
      context.SaveChanges(); 
    }
    }
}
