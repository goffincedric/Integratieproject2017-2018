using PB.BL;
using PB.BL.Domain.Items;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Newtonsoft.Json;
using System;
using PB.BL.Interfaces;

namespace UI_MVC.Controllers.API
{
    public class ItemController : ApiController
    {
        private UnitOfWorkManager UowMgr;
        private readonly IItemManager ItemMgr;

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
        public IHttpActionResult PostPerson([FromBody]Person person)
        {
            if (person == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (ItemMgr.GetItem(person.ItemId) != null) return Conflict();
            //Organisation organisation = ItemMgr.GetOrganisation(person.Organisation);
            person = ItemMgr.AddPerson(person.Name, person.SocialMediaLink, person.IconURL, person.Organisation, person.Function);

            return Ok(person); //Indien nodig aanpassen naar CreatedAtRoute om te redirecten naar pagina van gemaakte item
        }

        // POST: api/organisation
        [HttpPost]
        [Route("api/organisation")]
        [Authorize(Roles = "User,Admin,SuperAdmin")]
        public IHttpActionResult PostOrganisation([FromBody]Organisation organisation)
        {
            if (organisation == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (ItemMgr.GetItem(organisation.ItemId) != null) return Conflict();
            organisation = ItemMgr.AddOrganisation(organisation.Name, organisation.FullName, organisation.SocialMediaLink, organisation.IconURL);

            return Ok(organisation); //Indien nodig aanpassen naar CreatedAtRoute om te redirecten naar pagina van gemaakte item
        }

        // POST: api/theme
        [HttpPost]
        [Route("api/theme")]
        [Authorize(Roles = "User,Admin,SuperAdmin")]
        public IHttpActionResult PostTheme([FromBody]Theme theme)
        {
            if (theme == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (ItemMgr.GetItem(theme.ItemId) != null) return Conflict();
            ItemMgr.AddTheme(theme.Name, theme.Description, theme.IconURL, theme.IsTrending);

            return Ok(theme); //Indien nodig aanpassen naar CreatedAtRoute om te redirecten naar pagina van gemaakte item
        }

        // PUT: api/item/5
        [HttpPut]
        [Authorize(Roles = "User,Admin,SuperAdmin")]
        public IHttpActionResult Put(int id, [FromBody]Item item)
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
        public IHttpActionResult Delete([FromBody]int? id)
        {
            if (id == null) return BadRequest("No Id provided");
            if (id < 0) return BadRequest("Wrong id has been provided");
            if (ItemMgr.GetItem((int)id) == null) NotFound();
            ItemMgr.RemoveItem((int)id);
            return StatusCode(HttpStatusCode.NoContent);
        }
        #endregion

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
        #endregion


        #region records

        [HttpGet]
        public IHttpActionResult GetRecordsFromPerson(int id)
        {
            Person item = ItemMgr.GetPerson(id);
            if (item == null) return StatusCode(HttpStatusCode.NoContent);
            return Ok(JsonConvert.SerializeObject(item.Records));
        }

        [HttpGet]
        public IHttpActionResult GetPersonsTop(int id)
        {
            IEnumerable<Person> persons = ItemMgr.GetPersons().OrderByDescending(o => o.Records.Count()).Take(id);
            Dictionary<string, int> personmap = new Dictionary<string, int>();
            persons.ToList().ForEach(p => { personmap.Add(p.Name, p.Records.Count()); });
            if (persons == null) return StatusCode(HttpStatusCode.NoContent);
            return Ok(personmap);
        }
        #endregion




        #region RelatieveStijging


        [HttpGet]
        public IHttpActionResult GetPersonIncrease()
        {
            IEnumerable<Person> persons = ItemMgr.GetPersons().OrderByDescending(p => p.Records.Count()).Take(4);
            Dictionary<string, string> stijgingmap = new Dictionary<string, string>();
            foreach (Person person in persons)
            {
                IEnumerable<Record> records = person.Records.ToList();

                double allDays = records.OrderByDescending(p => p.Date.Date).GroupBy(p => p.Date.Date).ToList().Take(4).Average(p => p.ToList().Count());
                DateTime last = records.OrderByDescending(p => p.Date).First().Date.Date;
                double lastDay = records.OrderByDescending(p => p.Date.Date).Where(p => p.Date.Date >= last).Count();
                string stijging = "";
                stijging = Math.Round(((lastDay - allDays) / allDays) * 100, 4) + "%";


                stijgingmap.Add(person.Name, stijging);
            }

            if (stijgingmap == null) return StatusCode(HttpStatusCode.NoContent);
            return Ok(stijgingmap);
        }
        #endregion

        #region SentimentAnalyse

        [HttpGet]
        public IHttpActionResult GetPersonEvolution(int id)
        {
            Item item = ItemMgr.GetItem(id);
            if (item is Person)
            {
                IEnumerable<Record> records = ItemMgr.GetPerson(id).Records.Where(p => p.Sentiment.Polarity != 0.0).Where(o => o.Sentiment.Objectivity != 0).OrderByDescending(a => a.Date).Take(10);
                Dictionary<DateTime, double> recordsmap = new Dictionary<DateTime, double>();
                records.ToList().ForEach(p => { recordsmap.Add(p.Date, (p.Sentiment.Polarity * p.Sentiment.Objectivity)); });
                recordsmap.OrderBy(o => o.Key);
                if (records == null) return StatusCode(HttpStatusCode.NoContent);
                return Ok(recordsmap);
            }
            else
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
        }
        #endregion

        #region TweetEvolution
        [HttpGet]
        public IHttpActionResult GetItemTweet(int id)
        {
            Item item = ItemMgr.GetItem(id);
            List<Record> records = null; 
            if(item is Person person)
            {
                records.AddRange(person.Records.ToList());
            }
            else if(item is Organisation organisation)
            {
                records.AddRange(organisation.People.SelectMany(p => p.Records).Distinct().ToList()); 
                
            }
            else if (item is Theme theme)
            {
                records.AddRange(theme.Persons.SelectMany(p => p.Records).Distinct().ToList());
                records.AddRange(theme.Organisations.SelectMany(p => p.People.SelectMany(r=>r.Records).Distinct().ToList()));
            }

            records = records.Distinct().ToList(); 
            if (records == null) return NotFound();
            Dictionary<DateTime, int> recordsmap = new Dictionary<DateTime, int>();
                recordsmap = records.GroupBy(r => r.Date.Date).OrderByDescending(r => r.Key)
                            .ToDictionary(r => r.Key.Date, r => r.ToList().Count());
            
           
            
            if (recordsmap == null) return StatusCode(HttpStatusCode.NoContent);
            return Ok(recordsmap);
        }
        [HttpGet]
        public IHttpActionResult GetPersonTweet(int id)
        {
            IEnumerable<Record> records = ItemMgr.GetPerson(id).Records;
            if (records == null) return NotFound();
            Dictionary<DateTime, int> recordsmap = new Dictionary<DateTime, int>();

            recordsmap = records.GroupBy(r => r.Date.Date).OrderByDescending(r => r.Key)
            .ToDictionary(r => r.Key.Date, r => r.ToList().Count());
            if (recordsmap == null) return StatusCode(HttpStatusCode.NoContent);
            return Ok(recordsmap);
        }

        #endregion

        #region

        [HttpGet]
        public IHttpActionResult GetPersonAverageSentiment(int id)
        {
            IEnumerable<Record> records = ItemMgr.GetPerson(id).Records;
            if (records == null) return NotFound();
            Dictionary<DateTime, double> recordsmap = new Dictionary<DateTime, double>();

            recordsmap = records.GroupBy(r => r.Date.Date).OrderByDescending(r => r.Key).Take(10)
            .ToDictionary(r => r.Key.Date, r => (r.Average(p => p.Sentiment.Objectivity) * (r.Average(f => f.Sentiment.Polarity))));
            if (recordsmap == null) return StatusCode(HttpStatusCode.NoContent);
            return Ok(recordsmap);
        }


        [HttpGet]
        public IHttpActionResult GetTrendingMentions(int id)
        {
            if (ItemMgr.GetItem(id) is Person)
            {
                IEnumerable<Record> records = ItemMgr.GetRecordsFromItem(id);
                List<string> mentions = new List<string>();
                records.SelectMany(r => r.Mentions).Distinct().OrderByDescending(m => m.Records.Count).Take(12).ToList().ForEach(p => mentions.Add(p.Name));
                return Ok(mentions);
            }
            return NotFound();
        }

        [HttpGet]
        public IHttpActionResult GetTrendingHashtags(int id)
        {
            if (ItemMgr.GetItem(id) is Person)
            {
                IEnumerable<Record> records = ItemMgr.GetRecordsFromItem(id);
                List<string> hashtags = new List<string>();

                records.SelectMany(r => r.Hashtags).Distinct().OrderByDescending(h => h.Records.Count).Take(12).ToList().ForEach(p => hashtags.Add(p.HashTag));
                return Ok(hashtags);
            }
            return NotFound();
        }

<<<<<<< HEAD

        #endregion



=======
>>>>>>> master
        [HttpGet]
        public IHttpActionResult GetTrendingHashtagsCount(int id)
        {

            if (ItemMgr.GetItem(id) is Person)
            {
                IEnumerable<Record> records = ItemMgr.GetRecordsFromItem(id);
                Dictionary<string, int> hashtags = new Dictionary<string, int>();

                records.SelectMany(r => r.Hashtags).Distinct().OrderByDescending(h => h.Records.Count).Distinct().Take(5).ToList().ForEach(p => hashtags.Add(p.HashTag, p.Records.Count));
                return Ok(hashtags);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpGet]
        public IHttpActionResult GetTrendingMentionsCount(int id)
        {

            if (ItemMgr.GetItem(id) is Person)
            {
                IEnumerable<Record> records = ItemMgr.GetRecordsFromItem(id);
                Dictionary<string, int> mentions = new Dictionary<string, int>();

                records.SelectMany(r => r.Mentions).Distinct().OrderByDescending(h => h.Records.Count).Distinct().Take(5).ToList().ForEach(p => mentions.Add(p.Name, p.Records.Count));
                return Ok(mentions);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IHttpActionResult GetTrendingWordsCount(int id)
        {

            if (ItemMgr.GetItem(id) is Person)
            {
                IEnumerable<Record> records = ItemMgr.GetRecordsFromItem(id);
                Dictionary<string, int> words = new Dictionary<string, int>();

                records.SelectMany(r => r.Words).Distinct().OrderByDescending(h => h.Records.Count).Distinct().Take(5).ToList().ForEach(p => words.Add(p.Text, p.Records.Count));
                return Ok(words);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpGet]
        public IHttpActionResult GetTrendingUrl(int id)
        {
            if (ItemMgr.GetItem(id) is Person)
            {
                IEnumerable<Record> records = ItemMgr.GetRecordsFromItem(id);
                List<string> urls = new List<string>();
                records.SelectMany(r => r.URLs).Distinct().ToList().ForEach(p => urls.Add(p.Link));
                urls.Distinct();
                return Ok(urls);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IHttpActionResult GetMostPopularPerson()
        {
            Person person = ItemMgr.GetPersons().OrderByDescending(p => p.TrendingScore).FirstOrDefault();
            if (person is null)
            {
                return NotFound();
            }
            return Ok(person.ItemId);

        }

        [HttpGet]
        public IHttpActionResult GetMostPopularPersons()
        {
            Dictionary<int, string> ids = new Dictionary<int, string>();
            ItemMgr.GetPersons().OrderByDescending(p => p.Records.Count).ToList().Take(3).ToList().ForEach(p => ids.Add(p.ItemId, p.Name));

            return Ok(ids);

        }


    }
}