using EPAY.ETC.Core.API.Core.Interfaces.Services.Vehicles;
using EPAY.ETC.Core.API.Core.Models.Common;
using EPAY.ETC.Core.API.Core.Validation;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Infrastructure.Services
{
    public class VehicleHistoryService : IVehicleHistoryService
    {
        private readonly ILogger<VehicleHistoryService> _logger;
        private readonly IVehicleHistoryRepository _vehicleHistoryRepository;

        public VehicleHistoryService(ILogger<VehicleHistoryService> logger, IVehicleHistoryRepository VehicleHistoryRepository)
        {
            _logger = logger;
            _vehicleHistoryRepository = VehicleHistoryRepository;
        }

        //public async Task<ValidationResult<VehicleHistoryModel>> AddAsync(VehicleHistoryModel model)
        //{
        //    _logger.LogInformation($"Executing {nameof(AddAsync)} method...");
        //    try
        //    {
        //        var result = await _vehicleHistoryRepository.AddAsync(model);
        //        return ValidationResult.Success(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Failed to run {nameof(AddAsync)} method. Error: {ex.Message}");
        //        throw;
        //    }
        //}
    }
}
