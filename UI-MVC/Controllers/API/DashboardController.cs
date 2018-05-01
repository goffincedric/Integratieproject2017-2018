using PB.BL;
using PB.BL.Domain.Dashboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace UI_MVC.Controllers.API
{
    [Authorize(Roles = "User,Admin,SuperAdmin")]
    public class DashboardController : ApiController
    {
        private readonly UnitOfWorkManager UowMgr;
        private readonly DashboardManager DashboardMgr;
        private readonly SubplatformManager SubplatformMgr;

        public DashboardController()
        {
            UowMgr = new UnitOfWorkManager();
            DashboardMgr = new DashboardManager(UowMgr);
            SubplatformMgr = new SubplatformManager(UowMgr);
        }

        // GET: api/dashboard/getdashboardzones/5
        [HttpGet]
        public IHttpActionResult GetDashboardZones(int id)
        {
            Dashboard dashboard = DashboardMgr.GetDashboard(id);
            if (dashboard == null) return StatusCode(HttpStatusCode.NoContent);
            if (dashboard.Zones.Count == 0) return StatusCode(HttpStatusCode.NoContent);
            return Ok(dashboard.Zones);
        }

        // PUT: api/dashboard/putzone/{zoneId}
        [HttpPut]
        public IHttpActionResult PutZone(int id, [FromBody]Zone zone)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (zone.ZoneId != id) return BadRequest();
            Zone oldZone = DashboardMgr.GetZone(id);
            if (oldZone == null) return NotFound();
            DashboardMgr.ChangeZone(zone);
            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/dashboard/postzone/{dashboardId}
        [HttpPost]
        public IHttpActionResult PostZone(int id, [FromBody]Zone zone)
        {
            if (zone == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (DashboardMgr.GetZone(zone.ZoneId) != null) return Conflict();

            Dashboard dashboard = DashboardMgr.GetDashboard(id);
            if (dashboard == null) return NotFound();
            if (zone.Elements == null || zone.Elements.Count == 0) DashboardMgr.AddZone(dashboard, zone.Title);
            else DashboardMgr.AddZone(dashboard, zone.Title, zone.Elements);
            
            return Ok(zone); //Indien nodig aanpassen naar CreatedAtRoute om te redirecten naar pagina van gemaakte zone
        }

        // DELETE: api/dashboard/deletezone
        [HttpDelete]
        public IHttpActionResult DeleteZone([FromBody]int? id)
        {
            if (id == null) return BadRequest("No Id provided");
            if (id < 0) return BadRequest("Wrong id has been provided");
            if (DashboardMgr.GetZone((int)id) == null) NotFound();
            DashboardMgr.RemoveZone((int)id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: api/dashboard/getzoneelements/5
        [HttpGet]
        public IHttpActionResult GetZoneElements(int id)
        {
            Zone zone = DashboardMgr.GetZone(id);
            if (zone == null) return StatusCode(HttpStatusCode.NoContent);
            if (zone.Elements.Count == 0) return StatusCode(HttpStatusCode.NoContent);
            return Ok(zone.Elements);
        }

        // PUT: api/dashboard/putelement/{zoneId}
        [HttpPut]
        public IHttpActionResult PutElement(int id, [FromBody]Element element)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (element.ElementId != id) return BadRequest();
            Element oldElement = DashboardMgr.GetElement(id);
            if (oldElement == null) return NotFound();
            DashboardMgr.ChangeElement(element);
            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/dashboard/postelement/{zoneId}
        [HttpPost]
        public IHttpActionResult PostElement(int id, [FromBody]Element element)
        {
            if (element == null) return BadRequest();
            if (DashboardMgr.GetElement(element.ElementId) != null) return Conflict();
            Zone zone = DashboardMgr.GetZone(id);
            if (zone == null) return NotFound();
            element.Zone = zone;
            if (!ModelState.IsValid) return BadRequest(ModelState);

            element = DashboardMgr.AddElement(zone, element.Comparison, element.X, element.Y, element.Width, element.Height, element.IsDraggable);
            zone.Elements.Add(element);

            return Ok(element); //Indien nodig aanpassen naar CreatedAtRoute om te redirecten naar pagina van gemaakte element
        }

        // DELETE: api/dashboard/deleteelement
        [HttpDelete]
        public IHttpActionResult DeleteElement([FromBody]int? id)
        {
            if (id == null) return BadRequest("No Id provided");
            if (id < 0) return BadRequest("Wrong id has been provided");
            if (DashboardMgr.GetElement((int)id) == null) NotFound();
            DashboardMgr.RemoveElement((int)id);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
