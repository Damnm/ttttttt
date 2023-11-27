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
    public class InfringedVehicleService : IInfringedVehicleService
    {

        #region Variables   -
        private readonly ILogger<InfringedVehicleService> _logger;
        private readonly IInfringedVehicleRepository _repository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public InfringedVehicleService(ILogger<InfringedVehicleService> logger, IInfringedVehicleRepository vehicleRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        #endregion

        public async Task<ValidationResult<List<InfringedVehicleInfoModel>>?> GetAllAsync(Expression<Func<InfringedVehicleModel, bool>>? expressison = null)
        {
            _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");
            try
            {
                var vehicles = await _repository.GetAllAsync(expressison);

                if (vehicles == null || vehicles.Count() == 0)
                {
                    return ValidationResult.Failed<List<InfringedVehicleInfoModel>>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }
                var result =  _mapper.Map<List<InfringedVehicleModel>, List<InfringedVehicleInfoModel>>(vehicles.ToList());
                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(GetAllAsync)} method. Error: {ex.Message}");
                throw;
            }
        }
    }
}
