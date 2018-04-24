namespace PB.DAL.EF
{
    internal class IntegratieDbConfiguration : System.Data.Entity.DbConfiguration
    {
        public IntegratieDbConfiguration()
        {
            SetDefaultConnectionFactory(new System.Data.Entity.Infrastructure.SqlConnectionFactory());
            SetProviderServices("System.Data.SqlClient", System.Data.Entity.SqlServer.SqlProviderServices.Instance);
            SetDatabaseInitializer<IntegratieDbContext>(new IntegratieDbInitializer());
        }
    }
}
