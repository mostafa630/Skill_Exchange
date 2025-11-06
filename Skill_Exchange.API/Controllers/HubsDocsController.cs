using Microsoft.AspNetCore.Mvc;
using Skill_Exchange.API.Helpers;

namespace Skill_Exchange.API.Controllers
{
    [ApiController]
    [Route("api/docs/hubs")]
    public class HubsDocsController : ControllerBase
    {
        /// <summary>
        /// helper to get signalR hubs docs automatically
        /// </summary>
        [HttpGet]
        public IActionResult GetAllHubs()
        {
            var hubs = SignalRHubExplorer.GetHubMethods();
            return Ok(hubs);
        }
    }
}
