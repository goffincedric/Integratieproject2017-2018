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
        public PartialViewResult _OrganisationPartialTable()
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
        public ActionResult _OrganisationPartialCreate(Organisation organisation)
        {

            if (ModelState.IsValid)
            {
                itemMgr.AddOrganisation(organisation.Name, organisation.SocialMediaLink, organisation.IconURL);
                return RedirectToAction("AdminCrud", "Home");
            }
            else
            {
                return View();

            }

        }

        #endregion

        #region keywords
        public PartialViewResult _KeywordPartialTable()
        {
            IEnumerable<Keyword> keywords = itemMgr.GetKeywords();
            return PartialView(keywords);
        }

        public ActionResult _KeywordPartialCreate()
        {

            return PartialView();
        }

        [HttpPost]
        public ActionResult DeleteKeyword(int id, FormCollection collection)
        {
            try
            {
                //itemMgr.R;

                return RedirectToAction("AdminCrud", "Home");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _KeywordPartialCreate(Theme theme)
        {

            if (ModelState.IsValid)
            {
                //itemMgr.AddKeyword(theme.Name);
                return RedirectToAction("AdminCrud");
            }
            else
            {
                return View();

            }

        }
        #endregion

        #region Thema
        public PartialViewResult _ThemaPartialTable()
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
        public ActionResult _ThemaPartialCreate(Theme theme)
        {

            if (ModelState.IsValid)
            {
                itemMgr.AddTheme(theme.Name, theme.Description);
                return RedirectToAction("AdminCrud");
            }
            else
            {
                return View();

            }

        }
        #endregion

        #region persons
        public PartialViewResult _PersonPartialTable()
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
        public ActionResult _PersonPartialCreate(Person person)
        {

            if (ModelState.IsValid)
            {
                itemMgr.AddPerson(person.Name, person.SocialMediaLink,person.IconURL, person.Organisation, person.Function);
                return RedirectToAction("AdminCrud");
            }
            else
            {
                return View();

            }

        }
        #endregion



        [HttpPost]
        public ActionResult DeleteItem(int id, FormCollection collection)
        {
            try
            {
                itemMgr.RemoveItem(id);

                return RedirectToAction("AdminCrud","Home");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Charts()
        {
            return View();
        }


    }
}