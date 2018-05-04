using PB.BL;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UI_MVC.Models;

namespace UI_MVC.Controllers
{
    /// <summary>   
    /// Controller for everything that has to handle with items (persons, organisations, themes)
    /// Authorized by all roles at the moment
    /// </summary
    [RequireHttps]
    [Authorize(Roles = "User,Admin,SuperAdmin")]
    public class ItemController : Controller
    {
        private UnitOfWorkManager uow;
        private readonly ItemManager itemMgr;
        private readonly SubplatformManager SubplatformMgr;


        public ItemController()
        {
            uow = new UnitOfWorkManager();
            itemMgr = new ItemManager(uow);
            SubplatformMgr = new SubplatformManager(uow);
        }

        #region organisation
        public ActionResult _OrganisationPartialTable(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            IEnumerable<Organisation> organisations = itemMgr.GetOrganisations().Where(o => o.SubPlatforms.Contains(Subplatform));
            return PartialView(organisations);
        }

        public ActionResult _OrganisationPartialCreate()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrganisationPartialCreate(string subplatform, Organisation organisation)
        {
            if (ModelState.IsValid)
            {
                Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
                itemMgr.AddOrganisation(organisation.Name, organisation.FullName, organisation.SocialMediaLink, organisation.IconURL, subplatform: Subplatform);
                return RedirectToAction("ItemBeheer", "Item");
            }
            else
            {
                return RedirectToAction("ItemBeheer", "Item");
            }

        }
        #endregion

        #region keywords
        public ActionResult _KeywordPartialTable(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            IEnumerable<Keyword> keywords = itemMgr.GetKeywords().Where(k => k.Items.FirstOrDefault(i => i.SubPlatforms.Contains(Subplatform)) != null);
            return PartialView(keywords);
        }

        public ActionResult _KeywordPartialCreate()
        {
            return PartialView();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KeywordPartialCreate(string subplatform, Keyword keyword)
        {
            if (ModelState.IsValid)
            {
                Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
                itemMgr.AddKeyword(keyword.Name, keyword.Items);
                return RedirectToAction("ItemBeheer", "Item");
            }
            else
            {
                return RedirectToAction("ItemBeheer", "Item");
            }

        }
        #endregion

        #region Thema
        public ActionResult _ThemaPartialTable(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            IEnumerable<Theme> themes = itemMgr.GetThemes().Where(t => t.SubPlatforms.Contains(Subplatform));
            return PartialView(themes);
        }

        public ActionResult _ThemaPartialCreate()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemaPartialCreate(string subplatform, Theme theme)
        {
            if (ModelState.IsValid)
            {
                Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
                itemMgr.AddTheme(theme.Name, theme.Description, theme.IconURL, theme.IsTrending, Subplatform);
                return RedirectToAction("ItemBeheer", "Item");
            }
            else
            {
                return RedirectToAction("ItemBeheer", "Item");
            }

        }
        #endregion

        #region persons
        public ActionResult _PersonPartialTable(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            IEnumerable<Person> persons = itemMgr.GetPersons().Where(p => p.SubPlatforms.Contains(Subplatform));
            return PartialView(persons);
        }

        public ActionResult _PersonPartialCreate()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersonPartialCreate(string subplatform, PersonEditModel personEditModel)
        {
            if (ModelState.IsValid)
            {
                Organisation organisation = null;
                if (personEditModel.OrganisationId != null && personEditModel.OrganisationId > 1)
                {
                     organisation = itemMgr.GetOrganisation((int)personEditModel.OrganisationId);
                    if (organisation == null)
                        return RedirectToAction("ItemBeheer", "Item");
                }
                Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
                itemMgr.AddPerson(personEditModel.Name, personEditModel.SocialMediaLink, personEditModel.IconURL, organisation, subplatform: Subplatform);
                return RedirectToAction("ItemBeheer", "Item");
            }
            else
            {
                return RedirectToAction("ItemBeheer", "Item");
            }
        }
        #endregion



        [HttpPost]
        public ActionResult DeleteItem(string subplatform, int id)
        {
            try
            {
                Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
                itemMgr.RemoveItem(id, Subplatform);

                return RedirectToAction("ItemBeheer", "Item");
            }
            catch
            {
                return RedirectToAction("ItemBeheer", "Item");
            }
        }

        public ActionResult Charts()
        {
            return View();
        }

        public ActionResult Charts2()
        {
            return View();
        }


        public ActionResult ItemBeheer()
        {
            return View();
        }



       
    }
}