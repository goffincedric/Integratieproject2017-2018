﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PB.BL;
using PB.BL.Domain.Accounts;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;
using PB.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using UI_MVC.Controllers.API.Helper_Code;
using UI_MVC.Models;

namespace UI_MVC.Controllers.API
{

    [Authorize(Roles = "User,Admin,SuperAdmin")]
    public class AccountController : ApiController
    {
        private readonly UnitOfWorkManager uow;
        private AccountManager _accountMgr;
        private IntegratieSignInManager _signInManager;
        private readonly SubplatformManager SubplatformMgr;


        public AccountController()
        {
            uow = new UnitOfWorkManager();
            SubplatformMgr = new SubplatformManager(uow);
        }

        public IntegratieSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<IntegratieSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public AccountManager UserManager
        {
            get
            {
                return _accountMgr ?? HttpContext.Current.GetOwinContext().GetUserManager<AccountManager>();
            }
            private set
            {
                _accountMgr = value;
            }
        }

        // GET: api/account/getalerts
        [HttpGet]
        public IHttpActionResult GetAlerts(string subplatformUrl = "politieke-barometer")
        {
            Subplatform subplatform = SubplatformMgr.GetSubplatform(subplatformUrl);
            if (subplatform is null) return BadRequest();

            List<ProfileAlert> profileAlerts = UserManager.GetProfileAlerts(subplatform, UserManager.GetProfile(Thread.CurrentPrincipal.Identity.GetUserId()));
            if (profileAlerts.Count == 0) return NotFound();
            return Ok(profileAlerts);
        }

        // GET: api/account/getalerts
        [HttpGet]
        public IHttpActionResult GetAlertsPreviousDays(int? previousDays, string subplatformUrl = "politieke-barometer")
        {
            if (previousDays is null) BadRequest();
            Subplatform subplatform = SubplatformMgr.GetSubplatform(subplatformUrl);
            if (subplatform is null) return BadRequest();
            List<ProfileAlert> profileAlerts = UserManager.GetProfileAlerts(subplatform, UserManager.GetProfile(User.Identity.GetUserName())).Where(pa => pa.TimeStamp.Date >= DateTime.Today.AddDays(-(int)previousDays)).ToList();
            if (profileAlerts.Count == 0) return NotFound();
            return Ok(profileAlerts);
        }

        [HttpPut]
        public IHttpActionResult ReadProfileAlert(int? profileAlertId)
        {
            if (profileAlertId is null) return BadRequest();
            ProfileAlert profileAlert = UserManager.GetProfileAlert((int)profileAlertId);
            if (!profileAlert.UserId.Equals(Thread.CurrentPrincipal.Identity.GetUserId())) return BadRequest();
            profileAlert.IsRead = true;
            UserManager.ChangeProfileAlert(profileAlert);
            return Ok(profileAlert);
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

        [HttpGet]
        public IHttpActionResult GetUserRate()
        {
            IEnumerable<Profile> profiles = UserManager.GetProfiles();
            if (profiles == null) return NotFound();
            Dictionary<DateTime, int> profileRate = new Dictionary<DateTime, int>();

            profileRate = profiles.GroupBy(r => r.CreatedOn.Date).OrderBy(r => r.Key)
            .ToDictionary(r => r.Key.Date, r => r.ToList().Count());
            if (profileRate == null) return StatusCode(HttpStatusCode.NoContent);
            return Ok(profileRate);
        }

    }
}
