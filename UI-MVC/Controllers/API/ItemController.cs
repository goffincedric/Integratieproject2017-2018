﻿using PB.BL;
using PB.BL.Domain.Items;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Newtonsoft.Json;
using System;

namespace UI_MVC.Controllers.API
{
    [Authorize]
    public class ItemController : ApiController
    {
        private UnitOfWorkManager UowMgr;
        private readonly ItemManager ItemMgr;

        public ItemController()
        {
            UowMgr = new UnitOfWorkManager();
            ItemMgr = new ItemManager(UowMgr);
        }

        // GET: api/item
        [HttpGet]
        public IHttpActionResult GetItem()
        {
            IEnumerable<Item> items = ItemMgr.GetItems();
            if (items.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
            return Ok(items.ToList());
        }

        // GET: api/item/5
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
        public IHttpActionResult PostPerson([FromBody]Person person)
        {
            if (person == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (ItemMgr.GetItem(person.ItemId) != null) return Conflict();
            person = ItemMgr.AddPerson(person.Name, person.SocialMediaLink, person.IconURL, person.Organisation, person.Function);

            return Ok(person); //Indien nodig aanpassen naar CreatedAtRoute om te redirecten naar pagina van gemaakte item
        }

        // POST: api/organisation
        [HttpPost]
        [Route("api/organisation")]
        public IHttpActionResult PostOrganisation([FromBody]Organisation organisation)
        {
            if (organisation == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (ItemMgr.GetItem(organisation.ItemId) != null) return Conflict();
            organisation = ItemMgr.AddOrganisation(organisation.Name, organisation.Description, organisation.SocialMediaLink, organisation.IconURL);

            return Ok(organisation); //Indien nodig aanpassen naar CreatedAtRoute om te redirecten naar pagina van gemaakte item
        }

        // POST: api/theme
        [HttpPost]
        [Route("api/theme")]
        public IHttpActionResult PostTheme([FromBody]Theme theme)
        {
            if (theme == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (ItemMgr.GetItem(theme.ItemId) != null) return Conflict();
            ItemMgr.AddTheme(theme.Name, theme.Description);

            return Ok(theme); //Indien nodig aanpassen naar CreatedAtRoute om te redirecten naar pagina van gemaakte item
        }

        // PUT: api/item/5
        [HttpPut]
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
        public IHttpActionResult Delete([FromBody]int? id)
        {
            if (id == null) return BadRequest("No Id provided");
            if (id < 0) return BadRequest("Wrong id has been provided");
            if (ItemMgr.GetItem((int)id) == null) NotFound();
            ItemMgr.RemoveItem((int)id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpGet]
        public IHttpActionResult GetRecordsFromPerson(int id)
        {
            Person item = ItemMgr.GetPerson(id);
            if (item == null) return StatusCode(HttpStatusCode.NoContent);
            return Ok(JsonConvert.SerializeObject(item.Records));
        }


        [HttpGet]
        public IHttpActionResult GetPersonsTop()
        {
            IEnumerable<Person> persons = ItemMgr.GetPersons().OrderByDescending(o => o.Records.Count()).Take(5);
            Dictionary<string, int> personmap = new Dictionary<string, int>();
            persons.ToList().ForEach(p => { personmap.Add(p.Name, p.Records.Count()); });
            if (persons == null) return StatusCode(HttpStatusCode.NoContent);
            return Ok(personmap);
        }

        [HttpGet]
        public IHttpActionResult GetPersonTweet(int itemId)
        {
            Person person = ItemMgr.GetPerson(itemId);
            if (person == null) return NotFound();
            Dictionary<DateTime, int> recordsmap = person.Records
                .GroupBy(r => r.Date.Date)
                .ToDictionary(r => r.Key, r => r.ToList().Count);
            return Ok(recordsmap);
        }

        [HttpGet]
        public IHttpActionResult GetPersonEvolution(int id)
        {
            Item item = ItemMgr.GetItem(id);
            if (item is Person)
            {
                IEnumerable<Record> records = ItemMgr.GetPerson(id).Records.Where(p => p.Sentiment.Polarity != 0.0).Where(o => o.Sentiment.Objectivity != 0).OrderByDescending(a => a.Date).Take(20);
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

       
    }
}