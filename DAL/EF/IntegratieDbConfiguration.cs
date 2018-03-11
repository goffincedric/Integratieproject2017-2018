using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PB.DAL.EF
{
  public class IntegratieDbConfiguration:System.Data.Entity.DbConfiguration
  {
    public IntegratieDbConfiguration()
    {
      this.SetDefaultConnectionFactory(new System.Data.Entity.Infrastructure.SqlConnectionFactory());
      this.SetProviderServices("System.Data.SqlClient", System.Data.Entity.SqlServer.SqlProviderServices.Instance);
     
    }


   
  }
}
