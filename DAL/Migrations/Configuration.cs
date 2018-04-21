namespace PB.DAL.Migrations
{
    using System.Data.Entity.Migrations;

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

            context.SaveChanges();
        }
    }
}
