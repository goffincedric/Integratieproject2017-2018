using PB.BL;
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
        private readonly ISubplatformManager SubplatformManager;
        public SubplatformController()
        {
            UowMgr = new UnitOfWorkManager();
            SubplatformManager = new SubplatformManager(UowMgr);
        }

        [HttpGet]
        public IHttpActionResult GetTags(string name)
        {
            IEnumerable<Item> items = ItemMgr.GetItems();
            if (items.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
            return Ok(items.ToList());
        }

    }
}
