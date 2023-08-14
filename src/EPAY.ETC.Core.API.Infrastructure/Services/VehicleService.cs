using EPAY.ETC.Core.API.Core.Interfaces.Services;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Core.Validation;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Logging;

namespace EPAY.ETC.Core.API.Infrastructure.Services
{
    internal class VehicleService : IVehicleService
    {

        #region Variables   -
        private readonly ILogger<VehicleService> _logger;
        private readonly IVehicleRepository _vehicleRepository;
        #endregion

        #region Constructor
        public VehicleService(ILogger<VehicleService> logger, IVehicleRepository vehicleRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        }
        #endregion
        public async Task<ValidationResult<VehicleModel>> AddAsync(VehicleModel entity)
        {
            _logger.LogInformation($"Executing {nameof(AddAsync)} method...");
            try
            {
                var vehicle = new VehicleModel()
                {
                    Id = Guid.NewGuid(),
                                     
                };
                var res = await _vehicleRepository.AddAsync(vehicle);
                return ValidationResult.Success(res);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(AddAsync)} method. Error: {ex.Message}");
                throw;
            }
        }
    }
}
