using FleetManagement.Application.Model.Request;
using FleetManagement.Application.Services.FleetManagementService.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Text.Json;


namespace FleetManagement.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IFleetManagementService _fleetManagementService;
        private readonly ILogger<VehiclesController> _logger;

        public VehiclesController(IFleetManagementService fleetManagementService, ILogger<VehiclesController> logger)
        {
            _fleetManagementService = fleetManagementService;
            _logger = logger;
        }

        [HttpPost, Route("{vehicle}/[action]")]
        public async Task<IActionResult> Distribute([FromBody] VehicleDeliveryRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(400);

            _logger.LogInformation(new EventId(1234), $"Vehicle distribution geted request. Request Model : {JsonSerializer.Serialize(request).ToString()}");
            var result = await _fleetManagementService.Distribute(request);
            return Ok(result);
        }
    }
}
