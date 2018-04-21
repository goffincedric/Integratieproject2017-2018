using PB.DAL.EF;

namespace PB.DAL
{
    public class UnitOfWork
  {

    private IntegratieDbContext ctx;

    internal IntegratieDbContext Context
    {
      get { return ctx ?? (ctx = new IntegratieDbContext(true)); }
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
