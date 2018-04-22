using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace UI_MVC.Controllers.API
{
    [Authorize]
    public class AccountController : ApiController
    {
        // GET: api/Account
        public string[] Get()
        {
            return new string[] { "value1", "value2" };
        }

        //// GET: api/Account/5
        //public IHttpActionResult Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Account
        //public IHttpActionResult Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Account/5
        //public IHttpActionResult Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Account/5
        //public IHttpActionResult Delete(int id)
        //{
        //}
    }
}
