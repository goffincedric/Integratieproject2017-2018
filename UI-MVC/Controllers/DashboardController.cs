using Microsoft.AspNet.Identity;
using PB.BL;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Platform;
using PB.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI_MVC.Controllers
{
    [RequireHttps]
    [Authorize(Roles = "User,Admin,SuperAdmin")]
    public class DashboardController : Controller
    {
        private static readonly UnitOfWorkManager uow = new UnitOfWorkManager();
        private readonly ItemManager itemMgr = new ItemManager(uow);
        private readonly DashboardManager dashboardMgr = new DashboardManager(uow);
        private readonly AccountManager accountMgr = new AccountManager(new IntegratieUserStore(uow.UnitOfWork), uow);
        private readonly SubplatformManager SubplatformMgr = new SubplatformManager(uow);

        public ActionResult Dashboard(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            var user = accountMgr.GetProfile(User.Identity.GetUserId());

            //add subplatform later
            Dashboard model = dashboardMgr.GetDashboards().FirstOrDefault(d => d.Profile.Id == user.Id && d.Subplatform.URL.ToLower().Equals(subplatform.ToLower()));
            if (model == null)
            {
                model = new Dashboard()
                {
                    Profile = user,
                    DashboardType = UserType.USER,
                    Subplatform = Subplatform,
                    Zones = new List<Zone>
                    {
                        new Zone()
                        {
                            Title = "Main Trends",
                            Elements = new List<Element>()
                            {
                                new Element()
                                {
                                    X = 0,
                                    Y = 3,
                                    Width = 5,
                                    Height = 5,
                                    Comparison = new Comparison()
                                },
                                new Element()
                                {
                                    X = 0,
                                    Y = 0,
                                    Width = 2,
                                    Height = 3,
                                    Comparison = new Comparison()
                                }
                            }
                        },
                        new Zone()
                        {
                            Title = "Personal Trends",
                            Elements = new List<Element>()
                            {
                                new Element()
                                {
                                    X = 0,
                                    Y = 3,
                                    Width = 6,
                                    Height = 5,
                                    Comparison = new Comparison()
                                },
                                new Element()
                                {
                                    X = 0,
                                    Y = 0,
                                    Width = 2,
                                    Height = 4,
                                    Comparison = new Comparison()
                                }
                            }
                        }
                    }
                };
                model = dashboardMgr.AddDashboard(model.Subplatform, model.Profile, model.DashboardType, model.Zones);
            };

            return View(model);
        }
    }
}