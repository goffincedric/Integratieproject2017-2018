﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Newtonsoft.Json;
using PB.BL;
using PB.BL.Domain.Items;
using PB.BL.Interfaces;

namespace UI_MVC.Controllers.API
{
    public class ItemController : ApiController
    {
        private readonly IItemManager ItemMgr;
        private readonly UnitOfWorkManager UowMgr;

        public ItemController()
        {
            UowMgr = new UnitOfWorkManager();
            ItemMgr = new ItemManager(UowMgr);
        }

        #region Items
        // GET: api/item/getitem
        [HttpGet]
        public IHttpActionResult GetItem()
        {
            IEnumerable<Item> items = ItemMgr.GetItems();
            if (items.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
            return Ok(items.ToList());
        }

        // GET: api/item/getitem/5
        [HttpGet]
        public IHttpActionResult GetItem(int id)
        {
            Item item = ItemMgr.GetItem(id);
            if (item == null) return StatusCode(HttpStatusCode.NoContent);
            return Ok(item);
        }

        // GET: api/item/getperson
        [HttpGet]
        public IHttpActionResult GetPerson()
        {
            IEnumerable<Person> person = ItemMgr.GetPersons();
            if (person.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
            return Ok(person);
        }

        // GET: api/item/getperson/5
        [HttpGet]
        public IHttpActionResult GetPerson(int id)
        {
            Person person = ItemMgr.GetPerson(id);
            if (person == null) return StatusCode(HttpStatusCode.NoContent);
            return Ok(person);
        }

        // GET: api/item/getorganisation
        [HttpGet]
        public IHttpActionResult GetOrganisation()
        {
            IEnumerable<Organisation> organisations = ItemMgr.GetOrganisations();
            if (organisations.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
            return Ok(organisations);
        }

        // GET: api/item/getorganisation/5
        [HttpGet]
        public IHttpActionResult GetOrganisation(int id)
        {
            Organisation organisation = ItemMgr.GetOrganisation(id);
            if (organisation == null) return StatusCode(HttpStatusCode.NoContent);
            return Ok(organisation);
        }

        public IHttpActionResult GetThemesNotInOrganisation(int id)
        {
            IEnumerable<Theme> themes = ItemMgr.GetOrganisation(id).Themes.ToList();
            IEnumerable<Theme> themesAll = null;
            if (themes is null || themes.Count() < 1)
                themesAll = ItemMgr.GetThemes().ToList();
            else
                themesAll = ItemMgr.GetThemes().ToList().Except(themes);

            if (themesAll is null || themesAll.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
            return Ok(themesAll);
        }

        // GET: api/item/gettheme
        [HttpGet]
        public IHttpActionResult GetTheme()
        {
            IEnumerable<Theme> themes = ItemMgr.GetThemes();
            if (themes.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
            return Ok(themes);
        }

        // GET: api/item/gettheme/5
        [HttpGet]
        public IHttpActionResult GetTheme(int id)
        {
            Theme theme = ItemMgr.GetTheme(id);
            if (theme == null) return StatusCode(HttpStatusCode.NoContent);
            return Ok(theme);
        }

        // POST: api/person
        [HttpPost]
        [Route("api/person")]
        [Authorize(Roles = "User,Admin,SuperAdmin")]
        public IHttpActionResult PostPerson([FromBody] Person person)
        {
            if (person == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (ItemMgr.GetItem(person.ItemId) != null) return Conflict();
            //Organisation organisation = ItemMgr.GetOrganisation(person.Organisation);
            person = ItemMgr.AddPerson(person.Name, person.SocialMediaLink, person.IconURL, person.IsTrending,
                person.FirstName, person.LastName, person.Level, person.Site, person.TwitterName, person.Position,
                person.District, person.Gemeente, person.Postalcode, person.Gender, person.Organisation, null,
                person.DateOfBirth);

            return
                Ok(person); //Indien nodig aanpassen naar CreatedAtRoute om te redirecten naar pagina van gemaakte item
        }

        // POST: api/organisation
        [HttpPost]
        [Route("api/organisation")]
        [Authorize(Roles = "User,Admin,SuperAdmin")]
        public IHttpActionResult PostOrganisation([FromBody] Organisation organisation)
        {
            if (organisation == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (ItemMgr.GetItem(organisation.ItemId) != null) return Conflict();
            organisation = ItemMgr.AddOrganisation(organisation.Name, organisation.FullName,
                organisation.SocialMediaLink, null, organisation.IconURL);

            return
                Ok(organisation); //Indien nodig aanpassen naar CreatedAtRoute om te redirecten naar pagina van gemaakte item
        }

        // POST: api/theme
        [HttpPost]
        [Route("api/theme")]
        [Authorize(Roles = "User,Admin,SuperAdmin")]
        public IHttpActionResult PostTheme([FromBody] Theme theme)
        {
            if (theme == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (ItemMgr.GetItem(theme.ItemId) != null) return Conflict();
            ItemMgr.AddTheme(theme.Name, theme.Description, theme.IconURL, theme.Keywords, theme.IsTrending);

            return
                Ok(theme); //Indien nodig aanpassen naar CreatedAtRoute om te redirecten naar pagina van gemaakte item
        }

        // PUT: api/item/5
        [HttpPut]
        [Authorize(Roles = "User,Admin,SuperAdmin")]
        public IHttpActionResult Put(int id, [FromBody] Item item)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (item.ItemId != id) return NotFound();
            if (ItemMgr.GetItem(id) == null) return NotFound();
            ItemMgr.ChangeItem(item);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/item/5
        [HttpDelete]
        [Authorize(Roles = "User,Admin,SuperAdmin")]
        public IHttpActionResult Delete([FromBody] int? id)
        {
            if (id == null) return BadRequest("No Id provided");
            if (id < 0) return BadRequest("Wrong id has been provided");
            if (ItemMgr.GetItem((int)id) == null) NotFound();
            ItemMgr.RemoveItem((int)id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        #endregion

        #region Item Details
        // TODO OMSCHRIJVING
        // Relatieve stijging
        // ERROR: SEQUENCE CONTAINS NO ELEMENTS
        [HttpGet]
        public IHttpActionResult GetPersonIncrease()
        {
            IEnumerable<Person> persons = ItemMgr.GetPersons().OrderByDescending(p => p.Records.Count()).Take(4);
            Dictionary<string, string> stijgingmap = new Dictionary<string, string>();

            foreach (Person person in persons)
            {
                List<Record> records = person.Records;
                if (records.Count == 0) continue;
                double allDays = records.OrderByDescending(p => p.Date.Date).GroupBy(p => p.Date.Date).Take(4).Average(p => p.ToList().Count());
                DateTime last = records.OrderByDescending(p => p.Date).First().Date.Date;
                double lastDay = records.OrderByDescending(p => p.Date.Date).Where(p => p.Date.Date >= last).Count();

                string stijging = Math.Round((lastDay - allDays) / allDays * 100, 2).ToString();

                if (double.Parse(stijging) < 0)
                {
                    stijging = stijging + "%";
                }
                else
                {
                    stijging = "+" + stijging + "%";
                }

                stijgingmap.Add(person.Name, stijging);
            }

            if (stijgingmap.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
            return Ok(stijgingmap);
        }

        // TODO OMSCHRIJVING + CLEANUP?
        [HttpGet]
        public IHttpActionResult GetItemDetails(int id)
        {
            Item item = ItemMgr.GetItem(id);
            Dictionary<string, string> details = new Dictionary<string, string>();
            List<Record> records = new List<Record>();
            if (item is Person person)
            {
                records.AddRange(person.Records);
            }
            else if (item is Organisation organisation)
            {
                records.AddRange(organisation.People.SelectMany(p => p.Records));
            }
            else if (item is Theme theme)
            {
                IEnumerable<Record> first = theme.Organisations.SelectMany(p => p.People.SelectMany(r => r.Records));
                records.AddRange(theme.Persons.SelectMany(p => p.Records).Except(first));
            }

            if (records is null || records.Count() == 0) return StatusCode(HttpStatusCode.NoContent);

            double allDays = records.OrderByDescending(p => p.Date.Date).GroupBy(p => p.Date.Date).ToList().Take(4)
                .Average(p => p.ToList().Count());
            DateTime last = records.OrderByDescending(p => p.Date).First().Date.Date;
            double lastDay = records.OrderByDescending(p => p.Date.Date).Where(p => p.Date.Date >= last).Count();
            string stijging = "";
            stijging = Math.Round((lastDay - allDays) / allDays * 100, 2).ToString();
            if (double.Parse(stijging) < 0)
                stijging = stijging + "%";
            else
                stijging = "+" + stijging + "%";

            details.Add("Activiteit", stijging);

            double allDays2 = records.OrderByDescending(p => p.Date.Date).ToList().Take(4)
                .Average(p => p.Sentiment.Objectivity * p.Sentiment.Polarity);

            double lastDay2 = records.OrderByDescending(p => p.Date.Date).Where(p => p.Date.Date >= last)
                .Average(p => p.Sentiment.Objectivity * p.Sentiment.Polarity);
            string stijging2 = "";
            stijging2 = Math.Round((lastDay2 - allDays2) / allDays2 * 100, 2).ToString();
            if (double.Parse(stijging2) < 0)
                stijging2 = stijging2 + "%";
            else
                stijging2 = "+" + stijging2 + "%";

            details.Add("Positiviteit", stijging2);

            double allDays3 = records.OrderByDescending(p => p.Date.Date).Take(10).Where(p => p.Retweet.Equals(true))
                .Count();

            double lastDay3 = records.OrderByDescending(p => p.Date.Date).Where(p => p.Date.Date >= last)
                .Where(p => p.Retweet.Equals(true)).Count();
            string stijging3 = "";
            stijging3 = Math.Round((lastDay3 - allDays3) / allDays3 * 100, 2).ToString();
            if (double.Parse(stijging3) < 0)
                stijging3 = stijging3 + "%";
            else
                stijging3 = "+" + stijging3 + "%";
            details.Add("Retweet", stijging3);

            if (details.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
            return Ok(details);
        }
        #endregion

        #region Words
        // Gets top 5 trending words and their respective amount of records
        [HttpGet]
        public IHttpActionResult GetTrendingWordsCount(int id)
        {
            // Get item and check if exists
            Item item = ItemMgr.GetItem(id);
            if (item is null) NotFound();

            // Gets records from item
            List<Record> records = new List<Record>();
            if (item is Person person)
            {
                records.AddRange(person.Records);
            }
            else if (item is Organisation organisation)
            {
                records.AddRange(organisation.People.SelectMany(p => p.Records));
            }
            else if (item is Theme theme)
            {
                IEnumerable<Record> first = theme.Organisations.SelectMany(p => p.People.SelectMany(r => r.Records));
                records.AddRange(theme.Persons.SelectMany(p => p.Records).Except(first));
            }

            if (records.Count() == 0) return StatusCode(HttpStatusCode.NoContent);

            Dictionary<string, int> words = records
                .SelectMany(r => r.Words).Distinct()
                .OrderByDescending(h => h.Records.Count)
                .Take(5)
                .ToDictionary(w => w.Text, w => w.Records.Count);

            if (words.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
            return Ok(words);
        }

        // Gets top 5 trending words and their respective amount of records
        [HttpGet]
        public IHttpActionResult GetTrendingWordsCountOverall(int id)
        {
            // Get item and check if exists
            Item item = ItemMgr.GetItem(id);
            if (item is null) NotFound();

            // Gets records from item
            List<Record> records = new List<Record>();
            if (item is Person person)
            {
                records.AddRange(person.Records);
            }
            else if (item is Organisation organisation)
            {
                records.AddRange(organisation.People.SelectMany(p => p.Records));
            }
            else if (item is Theme theme)
            {
                IEnumerable<Record> first = theme.Organisations.SelectMany(p => p.People.SelectMany(r => r.Records));
                records.AddRange(theme.Persons.SelectMany(p => p.Records).Except(first));
            }

            if (records.Count() == 0) return StatusCode(HttpStatusCode.NoContent);

            // Count words from all records
            Dictionary<string, int> words = new Dictionary<string, int>
            {
                {
                    item.Name, records.SelectMany(r => { return r.Words; }).Count()
                }
            };
            return Ok(words);
        }
        #endregion

        #region Urls
        // Gets 6 urls from records from requested item
        [HttpGet]
        public IHttpActionResult GetTrendingUrl(int id)
        {
            Item item = ItemMgr.GetItem(id);
            if (item is null) NotFound();

            List<Record> records = new List<Record>();
            if (item is Person person)
            {
                records.AddRange(person.Records);
            }
            else if (item is Organisation organisation)
            {
                records.AddRange(organisation.People.SelectMany(p => p.Records).ToList());
            }
            else if (item is Theme theme)
            {
                IEnumerable<Record> first = theme.Organisations.SelectMany(p => p.People.SelectMany(r => r.Records));
                records.AddRange(theme.Persons.SelectMany(p => p.Records).Except(first));
            }

            if (records.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
            List<string> urls = records.SelectMany(r => r.URLs).Select(u => u.Link).Distinct().Take(6).ToList();

            if (urls.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
            return Ok(urls);
        }
        #endregion


        [HttpGet]
        public IHttpActionResult GetAges(int id)
        {
            Item item = ItemMgr.GetItem(id);
            if (item is null) NotFound();
            List<Record> records = new List<Record>(); ;
            if (item is Person person)
            {
                records.AddRange(person.Records);
            }
            else if (item is Organisation organisation)
            {
                records.AddRange(organisation.People.SelectMany(p => p.Records));
            }
            else if (item is Theme theme)
            {
                IEnumerable<Record> first = theme.Organisations.SelectMany(p => p.People.SelectMany(r => r.Records));
                records.AddRange(theme.Persons.SelectMany(p => p.Records).Except(first));
            }

            if (records.Count() == 0) return StatusCode(HttpStatusCode.NoContent);

            Dictionary<string, int> ages = records.GroupBy(p => p.RecordProfile.Age).ToDictionary(p => p.ToList().First().RecordProfile.Age, p => p.ToList().Count);

            if (ages.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
            return Ok(ages);
        }

        [HttpGet]
        public IHttpActionResult GetGender(int id)
        {
            Item item = ItemMgr.GetItem(id);
            if (item is null) NotFound();

            List<Record> records = new List<Record>();
            if (item is Person person)
            {
                records.AddRange(person.Records);
            }
            else if (item is Organisation organisation)
            {
                records.AddRange(organisation.People.SelectMany(p => p.Records));
            }
            else if (item is Theme theme)
            {
                IEnumerable<Record> first = theme.Organisations.SelectMany(p => p.People.SelectMany(r => r.Records));
                records.AddRange(theme.Persons.SelectMany(p => p.Records).Except(first));
            }

            if (records.Count() == 0) return StatusCode(HttpStatusCode.NoContent);

            Dictionary<string, int> gender = records.GroupBy(p => p.RecordProfile.Gender).ToDictionary(p => p.ToList().First().RecordProfile.Gender, p => p.ToList().Count);
            if (gender.Count() == 0) return StatusCode(HttpStatusCode.NoContent);

            return Ok(gender);
        }

        #region Keywords

        public IHttpActionResult GetKeywords()
        {
            IEnumerable<Keyword> keywords = ItemMgr.GetKeywords();
            if (keywords.Count() == 0) return NotFound();
            return Ok(keywords);
        }

        public IHttpActionResult GetKeywords(int itemId)
        {
            IEnumerable<Keyword> keywords = ItemMgr.GetKeywords(itemId);
            if (keywords.Count() == 0) return NotFound();
            return Ok(keywords);
        }

        public IHttpActionResult GetKeywordsNotInTheme(int id)
        {
            IEnumerable<Keyword> keywords = ItemMgr.GetTheme(id)?.Keywords;
            IEnumerable<Keyword> keywordsAll = null;
            if (keywords is null || keywords.Count() == 0)
                keywordsAll = ItemMgr.GetKeywords();
            else
                keywordsAll = ItemMgr.GetKeywords().Except(keywords);

            if (keywordsAll.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
            return Ok(keywordsAll);
        }

        #endregion

        #region Records
        [HttpGet]
        public IHttpActionResult GetRecordsFromPerson(int id)
        {
            Person item = ItemMgr.GetPerson(id);
            if (item == null) return StatusCode(HttpStatusCode.NoContent);
            return Ok(JsonConvert.SerializeObject(item.Records.Where(r => r.Date > DateTime.Today.AddDays(-14))));
        }

        [HttpGet]
        public IHttpActionResult GetPersonsTop(int id)
        {
            IEnumerable<Person> persons = ItemMgr.GetPersons().OrderByDescending(o => o.Records.Count()).Take(id);
            Dictionary<string, int> personmap = new Dictionary<string, int>();
            persons.ToList().ForEach(p => { personmap.Add(p.Name, p.Records.Count()); });
            if (persons == null || persons.Count() == 0) return NotFound();
            return Ok(personmap);
        }


        [HttpGet]
        public IHttpActionResult GetThemasTop(int id)
        {
            IEnumerable<Theme> themes = ItemMgr.GetThemes().OrderByDescending(o => o.Records.Count()).Take(id);
            Dictionary<string, int> themesmap = new Dictionary<string, int>();
            themes.ToList().ForEach(p => { themesmap.Add(p.Name, p.Records.Count()); });
            if (themes == null || themes.Count() == 0) return NotFound();
            return Ok(themesmap);
        }

        [HttpGet]
        public IHttpActionResult GetTweetsByDistrict()
        {
            return Ok(ItemMgr.GetTweetCountByDistrict());
        }

        #endregion

        #region SentimentAnalyse
        // Gets the average sentiment from item grouped by date from last 10 days
        [HttpGet]
        public IHttpActionResult GetPersonEvolution(int id)
        {
            Item item = ItemMgr.GetItem(id);
            List<Record> records = new List<Record>();

            if (item is Person person)
            {
                records.AddRange(ItemMgr.GetPerson(id).Records.Where(p =>
                {
                    return p.Sentiment.Polarity != 0.0 && p.Sentiment.Objectivity != 0 && p.Date > DateTime.Now.AddDays(-10);
                }).OrderByDescending(a => a.Date.Date));
            }
            else if (item is Organisation organisation)
            {
                records.AddRange(organisation.People.SelectMany(p => p.Records).Where(p => p.Sentiment.Polarity != 0.0 && p.Sentiment.Objectivity != 0 && p.Date > DateTime.Now.AddDays(-10)).OrderByDescending(a => a.Date.Date));
            }
            else if (item is Theme theme)
            {
                IEnumerable<Record> first = theme.Organisations.SelectMany(p => p.People.SelectMany(r => r.Records))
                    .Where(p => p.Sentiment.Polarity != 0.0 && p.Sentiment.Objectivity != 0 && p.Date > DateTime.Now.AddDays(-10)).OrderByDescending(a => a.Date.Date);
                records.AddRange(theme.Persons.SelectMany(p => p.Records).Where(p => p.Sentiment.Polarity != 0.0 && p.Sentiment.Objectivity != 0 && p.Date > DateTime.Now.AddDays(-10)).OrderByDescending(a => a.Date.Date).Except(first));
            }
            
            Dictionary<DateTime, double> recordsmap = records.GroupBy(r => r.Date.Date).ToDictionary(kv => kv.Key, kv => kv.ToList().Average(r => r.Sentiment.Polarity * r.Sentiment.Objectivity));

            for (int i = 0; i < 10; i++)
            {
                DateTime day = DateTime.Today.AddDays(-i);
                if (!recordsmap.ContainsKey(day))
                {
                    recordsmap.Add(day, 0);
                }
            }

            if (recordsmap.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
            recordsmap.OrderBy(o => o.Key);
            return Ok(recordsmap);
        }

        // Gets the average sentiment from item grouped by date (string-format) from last 10 days
        [HttpGet]
        public IHttpActionResult GetPersonEvolutionString(int id)
        {
            Item item = ItemMgr.GetItem(id);
            IEnumerable<Record> records = null;

            if (item is Person person)
            {
                records = ItemMgr.GetPerson(id).Records.Where(p => p.Sentiment.Polarity != 0.0 && p.Sentiment.Objectivity != 0 && p.Date > DateTime.Now.AddDays(-10)).OrderByDescending(a => a.Date.Date);
            }
            else if (item is Organisation organisation)
            {

                records = organisation.People.SelectMany(p => p.Records).Where(p => p.Sentiment.Polarity != 0.0 && p.Sentiment.Objectivity != 0 && p.Date > DateTime.Now.AddDays(-10)).OrderByDescending(a => a.Date.Date);
            }
            else if (item is Theme theme)
            {
                IEnumerable<Record> first = theme.Organisations.SelectMany(p => p.People.SelectMany(r => r.Records))
                    .Where(p => p.Sentiment.Polarity != 0.0 && p.Sentiment.Objectivity != 0 && p.Date > DateTime.Now.AddDays(10)).OrderByDescending(a => a.Date.Date);
                records = theme.Persons.SelectMany(p => p.Records).Where(p => p.Sentiment.Polarity != 0.0 && p.Sentiment.Objectivity != 0 && p.Date > DateTime.Now.AddDays(-10)).OrderByDescending(a => a.Date.Date).Except(first);
            }

            Dictionary<string, double> recordsmap = records.GroupBy(r => r.Date.Date.ToShortDateString()).ToDictionary(kv => kv.Key, kv => kv.ToList().Average(r => r.Sentiment.Polarity * r.Sentiment.Objectivity));

            for (int i = 0; i < 10; i++)
            {
                DateTime day = DateTime.Today.AddDays(-i);
                if (!recordsmap.ContainsKey(day.ToShortDateString()))
                {
                    recordsmap.Add(day.ToShortDateString(), 0);
                }
            }

            if (recordsmap.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
            recordsmap.OrderBy(o => o.Key);
            return Ok(recordsmap);
        }
        #endregion

        #region TweetEvolution
        // Gets amount of records from item grouped by date from last 10 days
        [HttpGet]
        public IHttpActionResult GetItemTweet(int id)
        {
            Item item = ItemMgr.GetItem(id);
            List<Record> records = new List<Record>();
            if (item is Person person)
            {
                records.AddRange(person.Records);
            }
            else if (item is Organisation organisation)
            {
                records.AddRange(organisation.People.SelectMany(p => p.Records).Distinct().ToList());
            }
            else if (item is Theme theme)
            {
                records.AddRange(theme.Persons.SelectMany(p => p.Records).Distinct().ToList());
                records.AddRange(theme.Organisations.SelectMany(p =>
                    p.People.SelectMany(r => r.Records).Distinct().ToList()));
            }

            records = records.Distinct().ToList();
            if (records == null) return StatusCode(HttpStatusCode.NoContent);
            Dictionary<DateTime, int> recordsmap = records
                .Where(r => r.Date > DateTime.Now.AddDays(-10))
                .GroupBy(r => r.Date.Date)
                .OrderBy(r => r.Key)
                .ToDictionary(r => r.Key.Date, r => r.ToList().Count());

            if (recordsmap.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
            return Ok(recordsmap);
        }

        // Gets amount of records from item grouped by date (string-format) from last 10 days
        [HttpGet]
        public IHttpActionResult GetItemTweetString(int id)
        {
            Item item = ItemMgr.GetItem(id);
            if (item == null) return StatusCode(HttpStatusCode.BadRequest);
            List<Record> records = new List<Record>();
            if (item is Person person)
            {
                records.AddRange(person.Records);
            }
            else if (item is Organisation organisation)
            {
                records.AddRange(organisation.People.SelectMany(p => p.Records).Distinct().ToList());
            }
            else if (item is Theme theme)
            {
                records.AddRange(theme.Persons.SelectMany(p => p.Records).Distinct().ToList());
                records.AddRange(theme.Organisations.SelectMany(p =>
                    p.People.SelectMany(r => r.Records).Distinct().ToList()));
            }

            records = records.Distinct().ToList();
            if (records == null) return StatusCode(HttpStatusCode.NoContent);
            Dictionary<string, int> recordsmap = records
                .Where(r => r.Date > DateTime.Now.AddDays(-10))
                .GroupBy(r => r.Date.Date)
                .OrderBy(r => r.Key)
                .ToDictionary(r => r.Key.Date.ToShortDateString(), r => r.ToList().Count());

            if (recordsmap.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
            return Ok(recordsmap);
        }

        // Gets amount of records from persons grouped by date
        [HttpGet]
        public IHttpActionResult GetPersonTweet(int id)
        {
            IEnumerable<Record> records = ItemMgr.GetPerson(id)?.Records;
            if (records is null) return NotFound();

            Dictionary<DateTime, int> recordsmap = records
                .GroupBy(r => r.Date.Date)
                .OrderByDescending(r => r.Key)
                .ToDictionary(r => r.Key.Date, r => r.ToList().Count());
            if (recordsmap == null || recordsmap.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
            return Ok(recordsmap);
        }

        #endregion

        #region Mentions
        // Gets 8 top trending mentions from person
        [HttpGet]
        public IHttpActionResult GetTrendingMentions(int id)
        {
            if (ItemMgr.GetItem(id) is Person person)
            {
                IEnumerable<string> mentions = person.Records.SelectMany(r => r.Mentions).Distinct().OrderByDescending(m => m.Records.Count).Take(8).Select(m => m.Name);                
                return Ok(mentions);
            }

            return NotFound();
        }

        [HttpGet]
        public IHttpActionResult GetTrendingMentionsCount(int id)
        {
            Item item = ItemMgr.GetItem(id);
            if (item == null) NotFound();

            List<Record> records = new List<Record>();
            if (item is Person person)
            {
                records.AddRange(person.Records);
            }
            else if (item is Organisation organisation)
            {
                records.AddRange(organisation.People.SelectMany(p => p.Records));
            }
            else if (item is Theme theme)
            {
                IEnumerable<Record> first = theme.Organisations.SelectMany(p => p.People.SelectMany(r => r.Records));
                records.AddRange(theme.Persons.SelectMany(p => p.Records).Except(first));
            }
            if (records.Count == 0) return NotFound();

            Dictionary<string, int> mentions = records.SelectMany(r => r.Mentions).Distinct().OrderByDescending(m => m.Records.Count).Take(5).ToDictionary(m => m.Name, m => m.Records.Count);
            if (mentions.Count() == 0) return NotFound();

            return Ok(mentions);
        }

        [HttpGet]
        public IHttpActionResult GetTrendingMentionsCountOverall(int id)
        {
            Item item = ItemMgr.GetItem(id);
            List<Record> records = new List<Record>();
            if (item is Person person)
            {
                records.AddRange(person.Records);
            }
            else if (item is Organisation organisation)
            {
                records.AddRange(organisation.People.SelectMany(p => p.Records));
            }
            else if (item is Theme theme)
            {
                IEnumerable<Record> first = theme.Organisations.SelectMany(p => p.People.SelectMany(r => r.Records));
                records.AddRange(theme.Persons.SelectMany(p => p.Records).Except(first));
            }

            if (records.Count == 0) return NotFound();
            Dictionary<string, int> mentions = new Dictionary<string, int>
            {
                { item.Name, records.SelectMany(r => { return r.Mentions; }).Count() }
            };

            if (mentions.Count() == 0) return NotFound();
            return Ok(mentions);
        }

        #endregion

        #region Hashtags

        [HttpGet]
        public IHttpActionResult GetTrendingHashtags(int id)
        {
            if (ItemMgr.GetItem(id) is Person)
            {
                IEnumerable<Record> records = ItemMgr.GetRecordsFromItem(id);
                if (records == null || records.Count() == 0) return NotFound();
                IEnumerable<string> hashtags = records.SelectMany(r => r.Hashtags).Distinct().OrderByDescending(h => h.Records.Count).Take(8).Select(h => h.HashTag);
                return Ok(hashtags);
            }

            return NotFound();
        }
        
        [HttpGet]
        public IHttpActionResult GetTrendingHashtagsCount(int id)
        {
            Item item = ItemMgr.GetItem(id);
            if (item is null) return NotFound();

            List<Record> records = new List<Record>();
            if (item is Person person)
            {
                records.AddRange(person.Records);
            }
            else if (item is Organisation organisation)
            {
                records.AddRange(organisation.People.SelectMany(p => p.Records));
            }
            else if (item is Theme theme)
            {
                IEnumerable<Record> first = theme.Organisations.SelectMany(p => p.People.SelectMany(r => r.Records));
                records.AddRange(theme.Persons.SelectMany(p => p.Records).Except(first));
            }

            if (records.Count == 0) return NotFound();

            Dictionary<string, int> hashtags = records.SelectMany(r => r.Hashtags).Distinct().OrderByDescending(h => h.Records.Count).Take(5).ToDictionary(h => h.HashTag, h => h.Records.Count);
            if (hashtags.Count() == 0) return StatusCode(HttpStatusCode.NoContent);

            return Ok(hashtags);
        }


        [HttpGet]
        public IHttpActionResult GetTrendingHashtagsCountOverall(int id)
        {
            Item item = ItemMgr.GetItem(id);
            if (item is null) return NotFound();

            List<Record> records = new List<Record>();
            if (item is Person person)
            {
                records.AddRange(person.Records);
            }
            else if (item is Organisation organisation)
            {
                records.AddRange(organisation.People.SelectMany(p => p.Records));
            }
            else if (item is Theme theme)
            {
                IEnumerable<Record> first = theme.Organisations.SelectMany(p => p.People.SelectMany(r => r.Records));
                records.AddRange(theme.Persons.SelectMany(p => p.Records).Except(first));
            }

            if (records.Count == 0) return NotFound();

            Dictionary<string, int> hashtags = new Dictionary<string, int>
            {
                { item.Name, records.SelectMany(r => { return r.Hashtags; }).Count() }
            };
            if (hashtags.Count() == 0) return NotFound();

            return Ok(hashtags);
        }

        #endregion

        #region Popularity
        [HttpGet]
        public IHttpActionResult GetMostPopularPerson()
        {
            Person person = ItemMgr.GetPersons().OrderByDescending(p => p.TrendingScore).FirstOrDefault();
            if (person is null) return NotFound();
            return Ok(person.ItemId);
        }

        [HttpGet]
        public IHttpActionResult GetTweetName()
        {
            Person person = ItemMgr.GetPersons().Where(p => !string.IsNullOrWhiteSpace(p.TwitterName)).OrderByDescending(p => p.TrendingScore).FirstOrDefault();
            if (person is null) return NotFound();
            return Ok(person.TwitterName);
        }

        [HttpGet]
        public IHttpActionResult GetMostPopularPersons(int? id)
        {
            Dictionary<int, string> ids = new Dictionary<int, string>();
            List<Person> Persons = ItemMgr.GetPersons().ToList();
            if (Persons is null || Persons.Count() == 0)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }

            if (Persons.Max(p => p.TrendingScore == 0))
                Persons.OrderByDescending(p => p.Records.Count).Take(id ?? 3).ToList()
                    .ForEach(p => ids.Add(p.ItemId, p.Name));
            else
                Persons.OrderByDescending(p => p.TrendingScore).Take(id ?? 3).ToList()
                    .ForEach(p => ids.Add(p.ItemId, p.Name));


            if (ids is null || ids.Count() == 0) return NotFound();
            return Ok(ids);
        }
        
        [HttpGet]
        public IHttpActionResult GetMostPopularOrganisations(int? id)
        {
            Dictionary<int, string> ids = new Dictionary<int, string>();
            List<Organisation> organisations = ItemMgr.GetOrganisations().ToList();
            if (organisations is null || organisations.Count() == 0)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            organisations.OrderByDescending(p => p.People.Sum(r => r.Records.Count)).Take(id ?? 3).ToList()
            .ForEach(p => ids.Add(p.ItemId, p.Name));

            if (ids is null || ids.Count() == 0) return NotFound();
            return Ok(ids);
        }

        [HttpGet]
        public IHttpActionResult GetPopularTweetName(int id)
        {
            Item item = ItemMgr.GetItem(id);
            Dictionary<string, string> map = new Dictionary<string, string>();
            if (item is Organisation organisation)
            {
                organisation.People.Where(p => !string.IsNullOrWhiteSpace(p.TwitterName)).OrderBy(r => r.TrendingScore).Take(4).ToList().ForEach(s => map.Add(s.Name, s.TwitterName));
            }
            else if (item is Theme theme)
            {
                theme.Persons.Where(p => !string.IsNullOrWhiteSpace(p.TwitterName)).OrderBy(r => r.TrendingScore).Take(4).ToList().ForEach(s => map.Add(s.Name, s.TwitterName));
            }

            if (map is null) return NotFound();
            return Ok(map);
        }
        #endregion
    }
}