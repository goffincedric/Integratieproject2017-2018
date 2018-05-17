using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;

namespace PB.DAL.EF
{
    internal class IntegratieDbConfiguration : DbConfiguration
    {
        public IntegratieDbConfiguration()
        {
            SetDefaultConnectionFactory(new SqlConnectionFactory());
            SetProviderServices("System.Data.SqlClient", SqlProviderServices.Instance);
            SetDatabaseInitializer(new IntegratieDbInitializer());
        }
    }
}