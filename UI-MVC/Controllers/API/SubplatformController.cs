﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using PB.BL;
using PB.BL.Domain.Platform;
using PB.BL.Interfaces;
using UI_MVC.Models;

namespace UI_MVC.Controllers.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SubplatformController : ApiController
    {
        private readonly ISubplatformManager SubplatformMgr;

        private readonly UnitOfWorkManager UowMgr;

        public SubplatformController()
        {
            UowMgr = new UnitOfWorkManager();
            SubplatformMgr = new SubplatformManager(UowMgr);
        }

        [HttpGet]
        public IHttpActionResult GetTagsOfMenu(int id)
        {
            Dictionary<string, string> tags = new Dictionary<string, string>();

            SubplatformMgr.GetPages().Where(p => p.PageName.Equals("Menu")).Where(p => p.SubplatformId == id).FirstOrDefault().Tags.ForEach(t => tags.Add(t.Name, t.Text));
            return Ok(tags);
        }
        [HttpPost]
        public IHttpActionResult ChangeTagMenu([FromBody]MenuViewModel model)
        {
            Subplatform subplatform = SubplatformMgr.GetSubplatform(model.Subplatform);

            Page page = SubplatformMgr.GetPages().Where(p => p.PageName.Equals("Menu")).Where( p=> p.SubplatformId == subplatform.SubplatformId).FirstOrDefault();
            Tag tag = page.Tags.Find(t => t.Name.ToLower().Equals(model.MenuItem.ToLower()));
            tag.Text = model.MenuText;
            SubplatformMgr.ChangeTag(tag);


            return Ok();
        }

       
        [HttpPost]
        public IHttpActionResult ChangeTagFAQ([FromBody]FAQViewModel model)
        {
            Subplatform subplatform = SubplatformMgr.GetSubplatform(model.Subplatform);

            Page page = SubplatformMgr.GetPages().Where(p => p.PageName.ToLower().Equals("FAQ".ToLower())).Where(p => p.SubplatformId == subplatform.SubplatformId).FirstOrDefault();
            Tag tag = page.Tags.Find(t => t.Name.ToLower().Equals(model.FAQitem.ToLower()));
            tag.Text = model.Question;
            tag.Text2 = model.Answer;
            SubplatformMgr.ChangeTag(tag);


            return Ok();
        }

        [HttpGet]
        public IHttpActionResult GetTagsFaq(int id)
        {
            Dictionary<string, Dictionary<string, string>> tags = new Dictionary<string, Dictionary<string, string>>();
            SubplatformMgr.GetPage(id).Tags.ToList().ForEach(p => tags.Add(p.Name, new Dictionary<string, string>() { { p.Text, p.Text2 } }));

            return Ok(tags);
        }

        [HttpPost]
        public IHttpActionResult AddQuestion([FromBody]FAQViewModel model)
        {
            Subplatform subplatform = SubplatformMgr.GetSubplatform(model.Subplatform);

            Page page = SubplatformMgr.GetPages().Where(p => p.PageName.ToLower().Equals("FAQ".ToLower())).Where(p => p.SubplatformId == subplatform.SubplatformId).FirstOrDefault();
            int count = page.Tags.Count() + 1;
          
            SubplatformMgr.AddTag(page.PageId,"Question" + count, model.Question, model.Answer);


            return Ok();
        }
    }
}