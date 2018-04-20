namespace PB.DAL.EF
{
    internal class IntegratieDbConfiguration : System.Data.Entity.DbConfiguration
    {
        public IntegratieDbConfiguration()
        {
            this.SetDefaultConnectionFactory(new System.Data.Entity.Infrastructure.SqlConnectionFactory());
            this.SetProviderServices("System.Data.SqlClient", System.Data.Entity.SqlServer.SqlProviderServices.Instance);
            this.SetDatabaseInitializer<IntegratieDbContext>(new IntegratieDbInitializer());
        }

    }
}
