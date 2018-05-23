using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PB.BL;
using PB.BL.Domain.Accounts;
using PB.BL.Domain.Platform;
using PB.BL.Interfaces;

namespace UI_MVC.Controllers.API
{
    [Authorize(Roles = "User,Admin,SuperAdmin")]
    public class AccountController : ApiController
    {
        private readonly SubplatformManager SubplatformMgr;
        private readonly UnitOfWorkManager uow;
        private AccountManager _accountMgr;
        private IntegratieSignInManager _signInManager;


        public AccountController()
        {
            uow = new UnitOfWorkManager();
            SubplatformMgr = new SubplatformManager(uow);
        }

        public IntegratieSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.Current.GetOwinContext().Get<IntegratieSignInManager>();
            private set => _signInManager = value;
        }

        public AccountManager UserManager
        {
            get => _accountMgr ?? HttpContext.Current.GetOwinContext().GetUserManager<AccountManager>();
            private set => _accountMgr = value;
        }

        // GET: api/account/getalerts
        [HttpGet]
        public IHttpActionResult GetAlerts(string subplatformUrl = "politieke-barometer")
        {
            Subplatform subplatform = SubplatformMgr.GetSubplatform(subplatformUrl);
            if (subplatform is null) return BadRequest();

            List<ProfileAlert> profileAlerts =
                UserManager.GetWebAPIProfileAlerts(subplatform, User.Identity.GetUserId());
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
            List<ProfileAlert> profileAlerts = UserManager
                .GetWebAPIProfileAlerts(subplatform, User.Identity.GetUserName())
                .Where(pa => pa.TimeStamp.Date >= DateTime.Today.AddDays(-(int) previousDays)).ToList();
            if (profileAlerts.Count == 0) return NotFound();
            return Ok(profileAlerts);
        }

        [HttpPut]
        public IHttpActionResult HasReadProfileAlert(int? profileAlertId)
        {
            if (profileAlertId is null) return BadRequest();
            ProfileAlert profileAlert = UserManager.GetProfileAlert((int) profileAlertId);
            if (!profileAlert.UserId.Equals(Thread.CurrentPrincipal.Identity.GetUserId())) return BadRequest();
            profileAlert.IsRead = true;
            UserManager.ChangeProfileAlert(profileAlert);
            return Ok(profileAlert);
        }

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


        [HttpGet]
        public IHttpActionResult GetSubscriptions()
        {
            IEnumerable<string> subscriptions = UserManager.GetProfile(User.Identity.GetUserId()).Subscriptions.Select(p=>p.Name);
            if (subscriptions is null || subscriptions.Count() ==  0) return StatusCode(HttpStatusCode.NoContent);
            return Ok(subscriptions); 
          
        }


    }
}