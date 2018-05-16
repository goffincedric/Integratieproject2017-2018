using PB.BL;
using PB.BL.Domain.Platform;
using PB.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace UI_MVC.Controllers.API
{
    public class SubplatformController : ApiController
    {

        private readonly UnitOfWorkManager UowMgr;
        private readonly ISubplatformManager SubplatformMgr;
        public SubplatformController()
        {
            UowMgr = new UnitOfWorkManager();
            SubplatformMgr = new SubplatformManager(UowMgr);
        }

        [HttpPost]
        public IHttpActionResult GetTags([FromBody]String name)
        {
            Page page = SubplatformMgr.GetPage(name);
            IEnumerable<Tag> tags = page.Tags.ToList();
            Dictionary<int, string> tagmap = new Dictionary<int, string>();
            tags.ToList().ForEach(p => tagmap.Add(p.TagId, p.Name));
            if (tags.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
            return Ok(tagmap);
        }

    }
}
