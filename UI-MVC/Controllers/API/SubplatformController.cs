using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using PB.BL;
using PB.BL.Domain.Platform;
using PB.BL.Interfaces;
using UI_MVC.Models;

namespace UI_MVC.Controllers.API
{
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
        public IHttpActionResult GetTags(int id)
        {
            Dictionary<string, string> tags = new Dictionary<string, string>();

            SubplatformMgr.GetPage(id).Tags.ToList().ForEach(t => tags.Add(t.Name, t.Text));
            return Ok(tags);
        }
        [HttpPut]
        public IHttpActionResult ChangeTagMenu([FromBody]MenuViewModel model)
        {
           
            Page page = SubplatformMgr.GetPage(3);
            Tag tag = page.Tags.Find(t => t.Name.ToLower().Equals(model.MenuItem.ToLower()));
            tag.Text = model.MenuText;
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
    }
}