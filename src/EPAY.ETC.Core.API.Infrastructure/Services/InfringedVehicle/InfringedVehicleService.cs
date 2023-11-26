using AutoMapper;
using EPAY.ETC.Core.API.Core.Interfaces.Services.InfringedVehicle;
using EPAY.ETC.Core.API.Core.Models.InfringeredVehicle;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.InfringedVehicle;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Services.InfringedVehicle
{
    public class InfringedVehicle : IInfringedVehicleService
    {

        #region Variables   -
        private readonly ILogger<InfringedVehicle> _logger;
        private readonly IInfringedVehicleRepository _repository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public InfringedVehicle(ILogger<InfringedVehicle> logger, IInfringedVehicleRepository vehicleRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        #endregion

        public async Task<ValidationResult<List<InfringedVehicleInfoModel>>?> GetByRFIDOrPlateNumberAsync(string rfidOrPlateNumber)
        {
            _logger.LogInformation($"Executing {nameof(GetByRFIDOrPlateNumberAsync)} method...");
            try
            {
                if (string.IsNullOrEmpty(rfidOrPlateNumber)) return ValidationResult.Failed<List<InfringedVehicleInfoModel>>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });

                bool isRFID = rfidOrPlateNumber.Length >= 15 ? true : false;
                Expression<Func<InfringedVehicleModel, bool>>? expression = s => !string.IsNullOrEmpty(s.RFID) && s.RFID.Equals(rfidOrPlateNumber);

                if (!isRFID)
                {
                    expression = s => !string.IsNullOrEmpty(s.PlateNumber) && s.PlateNumber.Equals(rfidOrPlateNumber);
                }
                var vehicles = await _repository.GetInfringedVehicleAsync(expression);

                if (vehicles == null || vehicles.Count == 0)
                {
                    return ValidationResult.Failed<List<InfringedVehicleInfoModel>>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }
                var result =  _mapper.Map<List<InfringedVehicleModel>, List<InfringedVehicleInfoModel>>(vehicles);
                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(GetByRFIDOrPlateNumberAsync)} method. Error: {ex.Message}");
                throw;
            }
        }
    }
}
