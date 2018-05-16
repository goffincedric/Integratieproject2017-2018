using PB.BL;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using PB.BL.Domain.Settings;
using System.Collections.Generic;
using System.IO;
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
    [OutputCache(Duration = 3600, VaryByParam = "none")]
    public class ItemController : Controller
    {
        private readonly UnitOfWorkManager uow;
        private readonly ItemManager itemMgr;
        private readonly SubplatformManager SubplatformMgr;

        public ItemController()
        {
            uow = new UnitOfWorkManager();
            itemMgr = new ItemManager(uow);
            SubplatformMgr = new SubplatformManager(uow);
            ViewBag.Home = SubplatformMgr.GetTag("Home").Text;
            ViewBag.Dashboard = SubplatformMgr.GetTag("Dashboard").Text;
            ViewBag.WeeklyReview = SubplatformMgr.GetTag("Weekly_Review").Text;
            ViewBag.MyAccount = SubplatformMgr.GetTag("Account").Text;
            ViewBag.More = SubplatformMgr.GetTag("More").Text;
            ViewBag.FAQ = SubplatformMgr.GetTag("FAQ").Text;
            ViewBag.Contact = SubplatformMgr.GetTag("Contact").Text;
            ViewBag.Legal = SubplatformMgr.GetTag("Legal").Text;
        }

        #region Organisation
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
        public ActionResult OrganisationPartialCreate(string subplatform, OrganisationEditModel organisationEditModel)
        {
            if (ModelState.IsValid)
            {
                Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
                var iconUrl = "";
                string _FileName = "";
                if (organisationEditModel.file != null)
                {
                    if (organisationEditModel.file.ContentLength > 0)
                    {
                        _FileName = Path.GetFileName(organisationEditModel.file.FileName);
                        var username = organisationEditModel.Name.ToString();
                        var newName = username + "." + _FileName.Substring(_FileName.IndexOf(".") + 1);
                        string _path = Path.Combine(Server.MapPath("~/Content/Images/Organisations/"), newName);
                        organisationEditModel.file.SaveAs(_path);
                        iconUrl = @"~/Content/Images/Organisations/" + newName;
                    }
                }
                else
                {
                    iconUrl = Subplatform.Settings.Where(p => p.SettingName.Equals(Setting.Platform.DEFAULT_NEW_ITEM_ICON)).First().Value;
                }

                Theme theme = null;

                if (organisationEditModel.ThemeId != null)
                {
                    theme = itemMgr.GetTheme((int)organisationEditModel.ThemeId);
                    itemMgr.AddOrganisation(organisationEditModel.Name, organisationEditModel.FullName, organisationEditModel.SocialMediaLink, new List<Theme> { theme }, iconUrl, subplatform: Subplatform);
                }
                else
                {
                    itemMgr.AddOrganisation(organisationEditModel.Name, organisationEditModel.FullName, organisationEditModel.SocialMediaLink, null, iconUrl, subplatform: Subplatform);
                }
                

             
                



                return RedirectToAction("ItemBeheer", "Item");
            }

            return RedirectToAction("ItemBeheer", "Item");


        }

        public ActionResult _ShowThemesOfOrganisation(int id)
        {
            IEnumerable<Theme> theme = itemMgr.GetOrganisation(id).Themes.ToList();
            return PartialView(theme);
        }

        [HttpGet]
        public ActionResult EditOrganisation(int id)
        {
            Organisation item = itemMgr.GetOrganisation(id);
            OrganisationEditModel organisation = new OrganisationEditModel()
            {
                Name = item.Name,
                FullName = item.FullName,
                IsTrending = item.IsTrending,
                SocialMediaLink = item.SocialMediaLink,
                ItemId = item.ItemId
            };
            return View(organisation);
        }

        [HttpPost]
        public ActionResult EditOrganisation(string subplatform, int id, OrganisationEditModel organisationEditModel)
        {
            if (ModelState.IsValid)
            {
                Organisation organisation = itemMgr.GetOrganisation(organisationEditModel.ItemId);
                var iconUrl = "";
                string _FileName = "";
                if (organisationEditModel.file != null)
                {
                    if (organisationEditModel.file.ContentLength > 0)
                    {
                        _FileName = Path.GetFileName(organisationEditModel.file.FileName);
                        var username = organisationEditModel.Name.ToString();
                        var newName = username + "." + _FileName.Substring(_FileName.IndexOf(".") + 1);
                        string _path = Path.Combine(Server.MapPath("~/Content/Images/Persons/"), newName);
                        organisationEditModel.file.SaveAs(_path);
                        iconUrl = @"~/Content/Images/Organisation/" + newName;
                        organisation.IconURL = iconUrl;
                    }
                }
                else
                {
                    iconUrl = organisation.IconURL;
                }
                organisation.IsTrending = organisationEditModel.IsTrending;
                organisation.FullName = organisationEditModel.FullName;
                organisation.SocialMediaLink = organisationEditModel.SocialMediaLink;
                organisation.Name = organisationEditModel.Name;
                itemMgr.ChangeOrganisation(organisation);
                return RedirectToAction("ItemBeheer", "Item");
            }
            return RedirectToAction("ItemBeheer", "Item");
        }
        #endregion

        #region Keywords
        public ActionResult _KeywordPartialTable(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            IEnumerable<Keyword> keywords = itemMgr.GetKeywords();
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
            return RedirectToAction("ItemBeheer", "Item");
        }

        [HttpPost]
        public ActionResult DeleteKeyword(string subplatform, int id)
        {
            try
            {
                Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
                itemMgr.RemoveKeyword(id);

                return RedirectToAction("ItemBeheer", "Item");
            }
            catch
            {
                return RedirectToAction("ItemBeheer", "Item");
            }
        }

        [HttpGet]
        public ActionResult EditKeyword(int id)
        {
            Keyword keyword = itemMgr.GetKeyword(id);
            return View(keyword);
        }


        [HttpPost]
        public ActionResult EditKeyword(string subplatform, int id)
        {
            Keyword keyword = itemMgr.GetKeyword(id);
            return View(keyword);
        }


        public ActionResult _ShowKeywordOfTheme(int id)
        {
            IEnumerable<Keyword> keywords = itemMgr.GetTheme(id).Keywords.ToList();
            return PartialView(keywords);
        }
        #endregion

        #region Themes
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
        public ActionResult ThemaPartialCreate(string subplatform, ThemeEditModel themeEditModel)
        {
            if (ModelState.IsValid)
            {
                Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
                var iconUrl = "";
                string _FileName = "";
                if (themeEditModel.file != null)
                {
                    if (themeEditModel.file.ContentLength > 0)
                    {
                        _FileName = Path.GetFileName(themeEditModel.file.FileName);
                        var username = themeEditModel.Name.ToString();
                        var newName = username + "." + _FileName.Substring(_FileName.IndexOf(".") + 1);
                        string _path = Path.Combine(Server.MapPath("~/Content/Images/Themes/"), newName);
                        themeEditModel.file.SaveAs(_path);
                        iconUrl = @"~/Content/Images/Themes/" + newName;
                    }
                }
                else
                {
                    iconUrl = Subplatform.Settings.Where(p => p.SettingName.Equals(Setting.Platform.DEFAULT_NEW_ITEM_ICON)).First().Value;
                }
                if(themeEditModel.KeywordId is null)
                {
                    Theme theme = itemMgr.AddTheme(themeEditModel.Name, themeEditModel.Description, iconUrl, new List<Keyword>(), themeEditModel.IsTrending, Subplatform);
                }
                else
                {
                    Keyword keyword = itemMgr.GetKeyword((int)themeEditModel.KeywordId);
                    Theme theme = itemMgr.AddTheme(themeEditModel.Name, themeEditModel.Description, iconUrl, new List<Keyword> { keyword }, themeEditModel.IsTrending, Subplatform);
                }
                
                
                return RedirectToAction("ItemBeheer", "Item");
            }
            return RedirectToAction("ItemBeheer", "Item");

        }

        [HttpGet]
        public ActionResult EditTheme(int id)
        {
            Theme item = itemMgr.GetTheme(id);
            ThemeEditModel themeEditModel = new ThemeEditModel()
            {
                ItemId = item.ItemId,
                Name = item.Name,
                IsTrending = item.IsTrending,
                Description = item.Description
            };
            return View(themeEditModel);
        }

        [HttpPost]
        public ActionResult EditTheme(string subplatform, int id, ThemeEditModel themeEditModel)
        {
            if (ModelState.IsValid)
            {
                Theme theme = itemMgr.GetTheme(themeEditModel.ItemId);
                var iconUrl = "";
                string _FileName = "";
                if (themeEditModel.file != null)
                {
                    if (themeEditModel.file.ContentLength > 0)
                    {
                        _FileName = Path.GetFileName(themeEditModel.file.FileName);
                        var username = themeEditModel.Name.ToString();
                        var newName = username + "." + _FileName.Substring(_FileName.IndexOf(".") + 1);
                        string _path = Path.Combine(Server.MapPath("~/Content/Images/Themes/"), newName);
                        themeEditModel.file.SaveAs(_path);
                        iconUrl = @"~/Content/Images/Themes/" + newName;
                        theme.IconURL = iconUrl;
                    }
                }
                else
                {
                    iconUrl = theme.IconURL;
                }
                theme.IsTrending = themeEditModel.IsTrending;
                theme.Name = themeEditModel.Name;
                theme.Description = themeEditModel.Description;
                Keyword keyword = itemMgr.GetKeyword((int)themeEditModel.KeywordId);
                theme.Keywords.Add(keyword);
                itemMgr.ChangeTheme(theme);
                return RedirectToAction("ItemBeheer", "Item");
            }
            return RedirectToAction("ItemBeheer", "Item");
        }
        #endregion

        #region Persons
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
                Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
                Organisation organisation = null;
                if (personEditModel.OrganisationId != null && personEditModel.OrganisationId >= 1)
                {
                    organisation = itemMgr.GetOrganisation((int)personEditModel.OrganisationId);
                    if (organisation == null)
                        return RedirectToAction("ItemBeheer", "Item");
                }
                var iconUrl = "";
                string _FileName = "";
                if (personEditModel.file != null)
                {
                    if (personEditModel.file.ContentLength > 0)
                    {
                        _FileName = Path.GetFileName(personEditModel.file.FileName);
                        var username = personEditModel.Name.ToString();
                        var newName = username + "." + _FileName.Substring(_FileName.IndexOf(".") + 1);
                        string _path = Path.Combine(Server.MapPath("~/Content/Images/Persons/"), newName);
                        personEditModel.file.SaveAs(_path);
                        iconUrl = @"~/Content/Images/Persons/" + newName;
                    }
                }
                else
                {
                    iconUrl = Subplatform.Settings.Where(p => p.SettingName.Equals(Setting.Platform.DEFAULT_NEW_ITEM_ICON)).First().Value;
                }


                itemMgr.AddPerson(personEditModel.Name, personEditModel.SocialMediaLink, iconUrl, organisation, null, personEditModel.IsTrending, Subplatform, personEditModel.Gemeente);
                return RedirectToAction("ItemBeheer", "Item");
            }
            return RedirectToAction("ItemBeheer", "Item");

        }

        [HttpGet]
        public ActionResult EditPerson(int id)
        {
            Person item = itemMgr.GetPerson(id);
            PersonEditModel person = new PersonEditModel()
            {
                Name = item.Name,
                IsTrending = item.IsTrending,
                SocialMediaLink = item.SocialMediaLink,
                Gemeente = item.Gemeente,
                ItemId = item.ItemId,
            };

            if (item.Organisation != null)
            {
                person.OrganisationId = item.Organisation.ItemId;
            }
            return View(person);
        }

        [HttpPost]
        public ActionResult EditPerson(string subplatform, int id, PersonEditModel personEditModel)
        {

            if (ModelState.IsValid)
            {
                Person person = itemMgr.GetPerson(personEditModel.ItemId);
                Organisation organisation = null;
                if (personEditModel.OrganisationId != null && personEditModel.OrganisationId >= 1)
                {
                    organisation = itemMgr.GetOrganisation((int)personEditModel.OrganisationId);
                    if (organisation == null)
                        return RedirectToAction("ItemBeheer", "Item");
                }
                Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
                var iconUrl = "";
                string _FileName = "";
                if (personEditModel.file != null)
                {
                    if (personEditModel.file.ContentLength > 0)
                    {
                        _FileName = Path.GetFileName(personEditModel.file.FileName);
                        var username = personEditModel.Name.ToString();
                        var newName = username + "." + _FileName.Substring(_FileName.IndexOf(".") + 1);
                        string _path = Path.Combine(Server.MapPath("~/Content/Images/Persons/"), newName);
                        personEditModel.file.SaveAs(_path);
                        iconUrl = @"~/Content/Images/Persons/" + newName;
                        person.IconURL = iconUrl;
                    }
                }
                else
                {
                    iconUrl = person.IconURL;
                }
                person.Gemeente = personEditModel.Gemeente;
                person.IsTrending = personEditModel.IsTrending;
                person.Organisation = organisation;
                person.SocialMediaLink = personEditModel.SocialMediaLink;
                person.Name = personEditModel.Name;
                itemMgr.ChangePerson(person);
                return RedirectToAction("ItemBeheer", "Item");
            }
            return RedirectToAction("ItemBeheer", "Item");
        }
        #endregion

        #region Items
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
        #endregion

        public ActionResult ItemBeheer()
        {
            return View();
        }

    }
}