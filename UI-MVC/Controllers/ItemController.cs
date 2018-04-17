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
            
            return View();
        }

        public ActionResult AdminCrud()
        {
            return View();
        }

        public PartialViewResult OrganisationPartialTable()
        {
            IEnumerable<Organisation> organisations = itemMgr.GetOrganisations();
            return PartialView(organisations);
        }


        public PartialViewResult ThemaPartialTable()
        {
            IEnumerable<Theme> themes = itemMgr.GetThemes();
            return PartialView(themes);
        }


        public PartialViewResult OrganisationPartialTableCreate(Organisation organisation)
        {
            if (ModelState.IsValid)
            {
                itemMgr.AddOrganisation(organisation.Name,organisation.SocialMediaLink,organisation.IconURL);
            }
            return PartialView();
        }
    }
}