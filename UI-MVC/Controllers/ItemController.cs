using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PB.BL;
using PB.BL.Domain.Items;

namespace UI_MVC.Controllers
{
    [RequireHttps]
  public class ItemController : Controller
  {

    private static readonly UnitOfWorkManager uow = new UnitOfWorkManager();
    private static readonly ItemManager itemMgr = new ItemManager(uow);

    public ActionResult ItemTables()
    {
      IEnumerable<Person> persons = itemMgr.GetPersons(); 
      return View(persons);
    }
  }
}