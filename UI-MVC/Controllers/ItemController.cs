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
  [Authorize(Roles = "User,Admin,SuperAdmin")]
  public class ItemController : Controller
  {

    private UnitOfWorkManager uow;
    private ItemManager itemMgr;


    public ItemController()
    {
      uow = new UnitOfWorkManager();
      itemMgr = new ItemManager(uow);

    }
    public ActionResult ItemTables()
    {
      IEnumerable<Person> persons = itemMgr.GetPersons();
      return View(persons);
    }
  }
}