using Microsoft.AspNet.Identity.Owin;
using PB.BL;
using PB.BL.Domain.Items;
using PB.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using UI_MVC.Models;

namespace UI_MVC.Controllers.API
{
    
    [Authorize(Roles = "User,Admin,SuperAdmin")]
    public class AccountController : ApiController
    {
        private static readonly UnitOfWorkManager uow = new UnitOfWorkManager();
        private AccountManager _accountMgr;
        private IntegratieSignInManager _signInManager;

        public AccountController()
        {

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


        // GET: api/Account
        [HttpGet]
        public string[] Get()
        {
            return new string[] { "value1", "value2" };
        }


        //[Route("api/login")]
        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult Login([FromBody]LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = SignInManager.PasswordSignIn(loginViewModel.Username, loginViewModel.Password, loginViewModel.RememberMe, true);

            switch (result)
            {
                case SignInStatus.Success:
                    return Ok();
                case SignInStatus.LockedOut:
                    return BadRequest("Locked out");
                case SignInStatus.Failure:
                    return BadRequest("Invalid login credentials");
                default:
                    return BadRequest("Login failed");
            }
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
