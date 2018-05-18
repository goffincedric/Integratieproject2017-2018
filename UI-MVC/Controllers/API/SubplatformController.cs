using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using PB.BL;
using PB.BL.Domain.Platform;
using PB.BL.Interfaces;

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

        [HttpPost]
        public IHttpActionResult GetTags([FromBody] string name)
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