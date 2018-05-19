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
    }
}