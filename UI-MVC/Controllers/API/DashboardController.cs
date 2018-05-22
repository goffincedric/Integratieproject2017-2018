using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PB.BL;
using PB.BL.Domain.Dashboards;
using PB.BL.Domain.Items;
using PB.BL.Domain.Platform;

namespace UI_MVC.Controllers.API
{
    [Authorize(Roles = "User,Admin,SuperAdmin")]
    public class DashboardController : ApiController
    {
        private readonly DashboardManager DashboardMgr;
        private readonly ItemManager ItemMgr;
        private readonly SubplatformManager SubplatformMgr;
        private readonly UnitOfWorkManager UowMgr;
        private AccountManager _accountMgr;

        public AccountManager UserManager
        {
            get => _accountMgr ?? HttpContext.Current.GetOwinContext().GetUserManager<AccountManager>();
            private set => _accountMgr = value;
        }

        public DashboardController()
        {
            UowMgr = new UnitOfWorkManager();
            DashboardMgr = new DashboardManager(UowMgr);
            ItemMgr = new ItemManager(UowMgr);
            SubplatformMgr = new SubplatformManager(UowMgr);
        }

        // GET: api/dashboard/getdashboardzones/5
        [HttpGet]
        public IHttpActionResult GetDashboardZones(int id)
        {
            Dashboard dashboard = DashboardMgr.GetDashboard(id);
            if (dashboard == null) return StatusCode(HttpStatusCode.NoContent);
            if (!dashboard.UserId.Equals(User.Identity.GetUserId())) return Unauthorized();
            if (dashboard.Zones.Count == 0) return StatusCode(HttpStatusCode.NoContent);
            return Ok(dashboard.Zones);
        }

        // GET: api/dashboard/getdashboardzones/politieke-barometer
        [HttpGet]
        public IHttpActionResult GetDashboardZones(string subplatform)
        {
            Subplatform Subplatform = SubplatformMgr.GetSubplatform(subplatform);
            Dashboard dashboard = UserManager.GetProfile(User.Identity.GetUserId()).Dashboards.FirstOrDefault(d =>
            {
                return d.SubplatformId == Subplatform.SubplatformId;
            });
            if (dashboard == null) return StatusCode(HttpStatusCode.NoContent);
            if (!dashboard.UserId.Equals(User.Identity.GetUserId())) return Unauthorized();
            if (dashboard.Zones.Count == 0) return StatusCode(HttpStatusCode.NoContent);
            Dictionary<int, string> zones = dashboard.Zones.ToDictionary(k => k.ZoneId, v => v.Title);
            return Ok(zones);
        }

        // PUT: api/dashboard/putzone/{zoneId}
        [HttpPut]
        public IHttpActionResult PutZone(int id, [FromBody] Zone zone)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (zone.ZoneId != id) return BadRequest();
            Zone newZone = DashboardMgr.GetZone(id);
            if (!newZone.Dashboard.UserId.Equals(User.Identity.GetUserId())) return Unauthorized();
            if (newZone == null) return NotFound();
            newZone.Title = zone.Title;
            DashboardMgr.ChangeZone(newZone);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/dashboard/postzone/{dashboardId}
        [HttpPost]
        public IHttpActionResult PostZone(int id, [FromBody] Zone zone)
        {
            if (zone == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (DashboardMgr.GetZone(zone.ZoneId) != null) return Conflict();

            Dashboard dashboard = DashboardMgr.GetDashboard(id);
            if (!dashboard.UserId.Equals(User.Identity.GetUserId())) return Unauthorized();
            if (dashboard == null) return NotFound();
            if (zone.Elements == null || zone.Elements.Count == 0) zone = DashboardMgr.AddZone(dashboard, zone.Title);
            else zone = DashboardMgr.AddZone(dashboard, zone.Title, zone.Elements);

            return Ok(zone); //Indien nodig aanpassen naar CreatedAtRoute om te redirecten naar pagina van gemaakte zone
        }

        // DELETE: api/dashboard/deletezone/zoneId
        [HttpDelete]
        public IHttpActionResult DeleteZone(int? id)
        {
            if (id == null) return BadRequest("No Id provided");
            if (id < 0) return BadRequest("Wrong id has been provided");
            Zone zone = DashboardMgr.GetZone((int)id);
            if (zone == null) NotFound();
            if (!zone.Dashboard.UserId.Equals(User.Identity.GetUserId())) return Unauthorized();
            Dashboard dashboard = DashboardMgr.GetDashboard(zone.DashboardId);
            dashboard.Zones.Remove(zone);
            DashboardMgr.RemoveZone((int)id);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: api/dashboard/getzoneelements/5
        [HttpGet]
        public IHttpActionResult GetZoneElements(int id)
        {
            Zone zone = DashboardMgr.GetZone(id);
            if (zone == null) return StatusCode(HttpStatusCode.NoContent);
            if (!zone.Dashboard.UserId.Equals(User.Identity.GetUserId())) return Unauthorized();
            if (zone.Elements.Count == 0) return StatusCode(HttpStatusCode.NoContent);
            return Ok(zone.Elements);
        }

        // GET: api/dashboard/getelement/5
        [HttpGet]
        public IHttpActionResult GetElement(int id)
        {
            Element element = DashboardMgr.GetElement(id);
            if (element == null) return StatusCode(HttpStatusCode.NoContent);
            if (!DashboardMgr.GetDashboard(element.Zone.DashboardId).UserId.Equals(User.Identity.GetUserId()))
                return Unauthorized();
            return Ok(element);
        }

        // PUT: api/dashboard/putelement/{elementId}
        [HttpPut]
        public IHttpActionResult PutElement(int id, [FromBody] Element element)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (element.ElementId != id) return BadRequest();
            Element newElement = DashboardMgr.GetElement(id);
            if (newElement == null) return NotFound();
            Zone newZone = DashboardMgr.GetZone(element.ZoneId);
            if (newZone == null) return NotFound();
            if (!newZone.Dashboard.UserId.Equals(User.Identity.GetUserId())) return Unauthorized();

            if (!(newElement.Height == element.Height && newElement.Width == element.Width &&
                  newElement.X == element.X && newElement.Y == element.Y && newElement.ZoneId == element.ZoneId &&
                  newElement.GraphType == element.GraphType))
            {
                newElement.Height = element.Height;
                newElement.Width = element.Width;
                newElement.X = element.X;
                newElement.Y = element.Y;
                newElement.ZoneId = element.ZoneId;
                newElement.Zone = newZone;
                newElement.GraphType = element.GraphType;
                newElement.IsUnfinished = element.IsUnfinished;
                newElement.DataType = element.DataType;
                if (element.Items != null)
                {
                    foreach (var item in element.Items)
                    {
                        Item addItem = ItemMgr.GetItem(item.ItemId);
                        newElement.Items.Add(addItem);
                    }
                }
                DashboardMgr.ChangeElement(newElement);
            }
            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/dashboard/postelement/{zoneId}
        [HttpPost]
        public IHttpActionResult PostElement(int id, [FromBody] Element element)
        {
            if (element == null) return BadRequest();
            if (DashboardMgr.GetElement(element.ElementId) != null) return Conflict();
            Zone zone = DashboardMgr.GetZone(id);
            if (zone == null) return NotFound();
            if (!zone.Dashboard.UserId.Equals(User.Identity.GetUserId())) return Unauthorized();
            element.Zone = zone;
            if (!ModelState.IsValid) return BadRequest(ModelState);

            element = DashboardMgr.AddElement(zone, element.X, element.Y, element.Width, element.Height,element.GraphType, element.IsUnfinished, isDraggable: element.IsDraggable);
            zone.Elements.Add(element);

            return
                Ok(element); //Indien nodig aanpassen naar CreatedAtRoute om te redirecten naar pagina van gemaakte element
        }

        // DELETE: api/dashboard/deleteelement
        [HttpDelete]
        public IHttpActionResult DeleteElement(int? id)
        {
            if (id == null) return BadRequest("No Id provided");
            if (id < 0) return BadRequest("Wrong id has been provided");
            if (DashboardMgr.GetElement((int)id) == null) NotFound();
            Element element = DashboardMgr.GetElement((int)id);

            DashboardMgr.RemoveElement((int)id);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}