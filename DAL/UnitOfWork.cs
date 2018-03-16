using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PB.DAL.EF;

namespace PB.DAL
{
  public class UnitOfWork
  {

    private IntegratieDbContext ctx;

    internal IntegratieDbContext Context
    {
      get
      {
        if (ctx == null) ctx = new IntegratieDbContext(true);
        return ctx;
      }
    }

    /// <summary>
    /// Deze methode zorgt ervoor dat alle tot hier toe aangepaste domein objecten
    /// worden gepersisteert naar de databank
    /// </summary>
    public void CommitChanges()
    {
      ctx.CommitChanges();
    }
  }
}
