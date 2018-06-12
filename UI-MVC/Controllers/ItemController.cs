using Domain.JSONConversion;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PB.BL;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using PB.BL.Domain.Settings;
using PB.DAL.EF;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
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
        private readonly UnitOfWorkManager uow;
        private readonly ItemManager itemMgr;
        private readonly AccountManager accountMgr;
        private readonly SubplatformManager SubplatformMgr;

        public ItemController()
        {
            uow = new UnitOfWorkManager();
            itemMgr = new ItemManager(uow);
            SubplatformMgr = new SubplatformManager(uow);
            accountMgr = new AccountManager(new IntegratieUserStore(uow.UnitOfWork), uow);

            if (System.Web.HttpContext.Current.Request.Url.Segments.Count() > 1)
            {
                Subplatform subplatform = SubplatformMgr.GetSubplatform(System.Web.HttpContext.Current.Request.Url.Segments[1].Trim('/'));

                IEnumerable<Tag> menuTags = subplatform.Pages.SingleOrDefault(p => p.PageName.Equals("Menu"))?.Tags;
                if (menuTags is null || menuTags.Count() == 0) return;
                ViewBag.Home = menuTags.SingleOrDefault(t => t.Name.Equals("Home"))?.Text ?? "Home";
                ViewBag.Dashboard = menuTags.SingleOrDefault(t => t.Name.Equals("Dashboard"))?.Text ?? "Dashboard";
                ViewBag.WeeklyReview = menuTags.SingleOrDefault(t => t.Name.Equals("Weekly_Review"))?.Text ?? "Weekly Review";
                ViewBag.MyAccount = menuTags.SingleOrDefault(t => t.Name.Equals("Account"))?.Text ?? "Account";
                ViewBag.More = menuTags.SingleOrDefault(t => t.Name.Equals("More"))?.Text ?? "More";
                ViewBag.FAQ = menuTags.SingleOrDefault(t => t.Name.Equals("FAQ"))?.Text ?? "FAQ";
                ViewBag.Contact = menuTags.SingleOrDefault(t => t.Name.Equals("Contact"))?.Text ?? "Contact";
                ViewBag.Legal = menuTags.SingleOrDefault(t => t.Name.Equals("Legal"))?.Text ?? "Legal";
                ViewBag.Items = menuTags.SingleOrDefault(t => t.Name.Equals("Items"))?.Text ?? "Items";
                ViewBag.Persons = menuTags.SingleOrDefault(t => t.Name.Equals("Persons"))?.Text ?? "Persons";
                ViewBag.Organisations = menuTags.SingleOrDefault(t => t.Name.Equals("Organisations"))?.Text ?? "Organisations";
                ViewBag.Themes = menuTags.SingleOrDefault(t => t.Name.Equals("Themes"))?.Text ?? "Themes";
            }
        }


        #region Organisation
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult _OrganisationPartialTable(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            IEnumerable<Organisation> organisations = itemMgr.GetOrganisations().Where(o => o.SubPlatforms.Contains(Subplatform));
            return PartialView(organisations);
        }



        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult _OrganisationPartialCreate()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult OrganisationPartialCreate(string subplatform, OrganisationEditModel organisationEditModel)
        {
            if (ModelState.IsValid)
            {
                Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
                var iconUrl = "";
                byte[] image = null;
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
                        iconUrl               = @"~/Content/Images/Organisations/" + newName;
                        ImageConverter imgCon = new ImageConverter();
                        var img               = Image.FromStream(organisationEditModel.file.InputStream);
                        image = (byte[])imgCon.ConvertTo(img, typeof(byte[]));
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
                    itemMgr.AddOrganisation(organisationEditModel.Name, organisationEditModel.FullName, organisationEditModel.SocialMediaLink, new List<Theme> { theme }, iconUrl, false, Subplatform, image);
                }
                else
                {
                    itemMgr.AddOrganisation(organisationEditModel.Name, organisationEditModel.FullName, organisationEditModel.SocialMediaLink, null, iconUrl, false, Subplatform, image);
                }

                return RedirectToAction("ItemBeheer", "Item");
            }

            return RedirectToAction("ItemBeheer", "Item");


        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult _ShowThemesOfOrganisation(int id)
        {
            IEnumerable<Theme> theme = itemMgr.GetOrganisation(id).Themes.ToList();
            return PartialView(theme);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
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
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult EditOrganisation(string subplatform, int id, OrganisationEditModel organisationEditModel)
        {
            if (ModelState.IsValid)
            {
                Organisation organisation = itemMgr.GetOrganisation(organisationEditModel.ItemId);
                var iconUrl = "";
                byte[] image = null;
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
                        organisation.IconURL = iconUrl;
                        ImageConverter imgCon = new ImageConverter();
                        var img = Image.FromStream(organisationEditModel.file.InputStream);
                        image = (byte[])imgCon.ConvertTo(img, typeof(byte[]));
                    }
                }
                else
                {
                    iconUrl = organisation.IconURL;
                }
                organisation.Image = image; 
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
        [Authorize(Roles = "Admin,SuperAdmin")]
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
        [Authorize(Roles = "Admin,SuperAdmin")]
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

        [Authorize(Roles = "Admin,SuperAdmin")]
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
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult EditKeyword(int id)
        {
            Keyword keyword = itemMgr.GetKeyword(id);
            return View(keyword);
        }


        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult EditKeyword(string subplatform, int id)
        {
            Keyword keyword = itemMgr.GetKeyword(id);
            return View(keyword);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult _ShowKeywordOfTheme(int id)
        {
            IEnumerable<Keyword> keywords = itemMgr.GetTheme(id).Keywords.ToList();
            return PartialView(keywords);
        }
        #endregion

        #region Themes
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult _ThemaPartialTable(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            IEnumerable<Theme> themes = itemMgr.GetThemes().Where(t => t.SubPlatforms.Contains(Subplatform));
            return PartialView(themes);
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult _ThemaPartialCreate()
        {
            return PartialView();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [ValidateAntiForgeryToken]
        public ActionResult ThemaPartialCreate(string subplatform, ThemeEditModel themeEditModel)
        {
            if (ModelState.IsValid)
            {
                Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
                var iconUrl = "";
                string _FileName = "";
                byte[] image = null;
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

                        ImageConverter imgCon = new ImageConverter();
                        var img = Image.FromStream(themeEditModel.file.InputStream);
                        image = (byte[])imgCon.ConvertTo(img, typeof(byte[]));
                    }
                }
                else
                {
                    iconUrl = Subplatform.Settings.Where(p => p.SettingName.Equals(Setting.Platform.DEFAULT_NEW_ITEM_ICON)).First().Value;
                }
                if (themeEditModel.KeywordId is null)
                {
                    Theme theme = itemMgr.AddTheme(themeEditModel.Name, themeEditModel.Description, iconUrl, new List<Keyword>(), themeEditModel.IsTrending,Subplatform, image);
                }
                else
                {
                    Keyword keyword = itemMgr.GetKeyword((int)themeEditModel.KeywordId);
                    Theme theme = itemMgr.AddTheme(themeEditModel.Name, themeEditModel.Description, iconUrl, new List<Keyword> { keyword }, themeEditModel.IsTrending, Subplatform, image);
                }


                return RedirectToAction("ItemBeheer", "Item");
            }
            return RedirectToAction("ItemBeheer", "Item");

        }

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
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
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult EditTheme(string subplatform, int id, ThemeEditModel themeEditModel)
        {
            byte[] image = null;

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
                        ImageConverter imgCon = new ImageConverter();
                        var img = Image.FromStream(themeEditModel.file.InputStream);
                        image = (byte[])imgCon.ConvertTo(img, typeof(byte[]));
                    }
                }
                else
                {
                    iconUrl = theme.IconURL;
                }
                theme.Image = image; 
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
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult _PersonPartialTable(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            IEnumerable<Person> persons = itemMgr.GetPersons().Where(p => p.SubPlatforms.Contains(Subplatform));
            return PartialView(persons);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult _PersonPartialCreate()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult PersonPartialCreate(string subplatform, PersonEditModel personEditModel)
        {
            byte[] image = null;
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
                        ImageConverter imgCon = new ImageConverter();
                        var img = Image.FromStream(personEditModel.file.InputStream);
                        image = (byte[])imgCon.ConvertTo(img, typeof(byte[]));
                    }
                }
                else
                {
                    iconUrl = Subplatform.Settings.Where(p => p.SettingName.Equals(Setting.Platform.DEFAULT_NEW_ITEM_ICON)).First().Value;
                }

                itemMgr.AddPerson(personEditModel.Name, personEditModel.SocialMediaLink, iconUrl, personEditModel.IsTrending, null, null, null, null, null, null, null, personEditModel.Gemeente, null, null, organisation, Subplatform, null, image);

                return RedirectToAction("ItemBeheer", "Item");
            }
            return RedirectToAction("ItemBeheer", "Item");

        }

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
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
                District = item.District, 
                Level = item.Level,
                Site = item.Site
            };

            if (item.Organisation != null)
            {
                person.OrganisationId = item.Organisation.ItemId;
            }
            return View(person);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult EditPerson(string subplatform, int id, PersonEditModel personEditModel)
        {
            byte[] image = null;
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
                        ImageConverter imgCon = new ImageConverter();
                        var img = Image.FromStream(personEditModel.file.InputStream);
                        image = (byte[])imgCon.ConvertTo(img, typeof(byte[]));

                    }
                }
                else
                {
                    iconUrl = person.IconURL;
                }
                person.Image = image; 
                person.Gemeente = personEditModel.Gemeente;
                person.IsTrending = personEditModel.IsTrending;
                person.Organisation = organisation;
                person.SocialMediaLink = personEditModel.SocialMediaLink;
                person.Name = personEditModel.Name;
                person.District = personEditModel.District;
                itemMgr.ChangePerson(person);
                return RedirectToAction("ItemBeheer", "Item");
            }
            return RedirectToAction("ItemBeheer", "Item");
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public JsonResult Export()
        {
            IEnumerable<Person> persons = itemMgr.GetPersons().ToList();
            var serializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            };
            string json = JsonConvert.SerializeObject(persons, serializerSettings);
            string _path = Path.Combine(Server.MapPath("~/Content/Images/Export/"), "Persons.json");
            System.IO.File.WriteAllText(_path, json);
            return Json(new { fileName = "Persons.json", errorMessage = "" });
        }

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult ExportPersons(string file)
        {
            string fullPath = Path.Combine(Server.MapPath("~/Content/Images/Export/"), file);
            return File(fullPath, "application/", file);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult Import(string subplatform, FileViewModel fileViewModel)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            if (ModelState.IsValid)
            {

                if (fileViewModel.file != null)
                {
                    if (fileViewModel.file.ContentLength > 0)
                    {
                        StreamReader stream = new StreamReader(fileViewModel.file.InputStream);
                        string x = stream.ReadToEnd();
                        List<Item> persons = itemMgr.JPersonToPerson(JsonConvert.DeserializeObject<List<JPerson>>(x), Subplatform);
                        itemMgr.AddItems(persons);


                    }
                }
            }


            return View("Itembeheer");
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
        [AllowAnonymous]
        public ActionResult ShowPersons()
        {
            ViewBag.Profile = accountMgr.GetProfile(User.Identity.GetUserId()); 
          
            IEnumerable<Person> people = itemMgr.GetPersons();
            return View(people);
        }
        [AllowAnonymous]
        public ActionResult ShowOrganisations()
        {
            ViewBag.Profile = accountMgr.GetProfile(User.Identity.GetUserId());
            IEnumerable<Organisation> organisations = itemMgr.GetOrganisations();
            return View(organisations);
        }
        [AllowAnonymous]
        public ActionResult ShowThemes()
        {
            ViewBag.Profile = accountMgr.GetProfile(User.Identity.GetUserId());
            IEnumerable<Theme> themes = itemMgr.GetThemes();
            return View(themes);
        }

        [Authorize(Roles = "User,Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSubscription(int id)
        {
            var user = accountMgr.GetProfile(User.Identity.GetUserId());
            Item item = itemMgr.GetItem(id);
            string redirect = "";
            if (item is Person person)
            {
                redirect = "ShowPersons";

            }else if(item is Organisation organisation)
            {
                redirect = "ShowOrganisations";
            }
            else if(item is Theme theme)
            {
                redirect = "ShowThemes";

            }
            if (!user.Subscriptions.Contains(item))
            {
                accountMgr.AddSubscription(user, item);
                
                return RedirectToAction(redirect,"Item");
            }

            return RedirectToAction(redirect, "Item");
        }

        [Authorize(Roles = "User,Admin,SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveSubscription(int id)
        {
            var user = accountMgr.GetProfile(User.Identity.GetUserId());
            Item item = itemMgr.GetItem(id);
            string redirect = "";
            if (item is Person person)
            {
                redirect = "ShowPersons";

            }
            else if (item is Organisation organisation)
            {
                redirect = "ShowOrganisations";
            }
            else if (item is Theme theme)
            {
                redirect = "ShowThemes";

            }
            if (user.Subscriptions.Contains(item))
            {
                accountMgr.RemoveSubscription(user, item);
                return RedirectToAction(redirect, "Item");
            }

            return RedirectToAction(redirect, "Item");
        }


    }
}