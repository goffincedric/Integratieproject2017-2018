using PB.BL;
using PB.BL.Domain.Items;
using System.Collections.Generic;
using System.Web.Mvc;

namespace UI_MVC.Controllers
{
    [RequireHttps]
    [Authorize(Roles = "User,Admin,SuperAdmin")]
    public class ItemController : Controller
    {
        private UnitOfWorkManager uow;
        private readonly ItemManager itemMgr;


        public ItemController()
        {
            uow = new UnitOfWorkManager();
            itemMgr = new ItemManager(uow);

        }
       

       

        #region organisation
        public ActionResult _OrganisationPartialTable()
        {
            IEnumerable<Organisation> organisations = itemMgr.GetOrganisations();
            return PartialView(organisations);
        }

        public ActionResult _OrganisationPartialCreate()
        {

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrganisationPartialCreate(Organisation organisation)
        {

            if (ModelState.IsValid)
            {
                itemMgr.AddOrganisation(organisation.Name, organisation.FullName, organisation.SocialMediaLink, organisation.IconURL);
                return RedirectToAction("ItemBeheer", "Item");
            }
            else
            {
                return RedirectToAction("ItemBeheer", "Item");

            }

        }

        #endregion

        #region keywords
        public ActionResult _KeywordPartialTable()
        {
            IEnumerable<Keyword> keywords = itemMgr.GetKeywords();
            return PartialView(keywords);
        }

        public ActionResult _KeywordPartialCreate()
        {

            return PartialView();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KeywordPartialCreate(Theme theme)
        {

            if (ModelState.IsValid)
            {
                itemMgr.AddKeyword(theme.Name);
                return RedirectToAction("ItemBeheer", "Item");
            }
            else
            {
                return RedirectToAction("ItemBeheer", "Item");

            }

        }
        #endregion

        #region Thema
        public ActionResult _ThemaPartialTable()
        {
            IEnumerable<Theme> themes = itemMgr.GetThemes();
            return PartialView(themes);
        }

        public ActionResult _ThemaPartialCreate()
        {

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemaPartialCreate(Theme theme)
        {

            if (ModelState.IsValid)
            {

                itemMgr.AddTheme(theme.Name, theme.Description, theme.IconURL, theme.IsTrending);
                return RedirectToAction("ItemBeheer", "Item");

            }
            else
            {
                return RedirectToAction("ItemBeheer", "Item");

            }

        }
        #endregion

        #region persons
        public ActionResult _PersonPartialTable()
        {
            IEnumerable<Person> persons = itemMgr.GetPersons();
            return PartialView(persons);
        }

        public ActionResult _PersonPartialCreate()
        {



            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersonPartialCreate(Person person)
        {

            if (ModelState.IsValid)
            {
                itemMgr.AddPerson(person.Name, person.SocialMediaLink,person.IconURL, person.Organisation,null);
                return RedirectToAction("ItemBeheer", "Item");
            }
            else
            {
                return RedirectToAction("ItemBeheer", "Item");

            }

        }
        #endregion



        [HttpPost]
        public ActionResult DeleteItem(int id)
        {
            try
            {
                itemMgr.RemoveItem(id);

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