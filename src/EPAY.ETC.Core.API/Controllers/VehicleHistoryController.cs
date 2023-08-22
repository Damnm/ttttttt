using EPAY.ETC.Core.API.Core.Interfaces.Services.Vehicles;
using EPAY.ETC.Core.API.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace EPAY.ETC.Core.API.Controllers
{
    [ApiController]
    [Route("~/api/[controller]/Stations/{stationId}")]
    public class VehicleHistoryController : ControllerBase
    {
        private readonly ILogger<VehicleHistoryController> _logger;
        private readonly IVehicleHistoryService _vehicleHistoryService;

        public VehicleHistoryController(ILogger<VehicleHistoryController> logger,
            IVehicleHistoryService vehicleHistoryService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _vehicleHistoryService = vehicleHistoryService ?? throw new ArgumentNullException(nameof(vehicleHistoryService));
        }
    }
}
