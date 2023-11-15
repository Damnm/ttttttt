using AutoMapper;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Transaction;
using EPAY.ETC.Core.API.Core.Models.TransactionLog;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TransactionLog;
using EPAY.ETC.Core.Models.Fees;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.Extensions.Logging;

namespace EPAY.ETC.Core.API.Infrastructure.Services.TransactionLog
{
    public class LaneInRFIDTransactionLogService : ILaneInRFIDTransactionLogService
    {

        #region Variables 
        private readonly ILogger<LaneInRFIDTransactionLogService> _logger;
        private readonly ILaneInRFIDTransactionLogRepository _repository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public LaneInRFIDTransactionLogService(ILogger<LaneInRFIDTransactionLogService> logger, ILaneInRFIDTransactionLogRepository repository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #endregion
        public async Task<ValidationResult<bool>> AddOrUpdateAsync(Guid? id, LaneInVehicleModel input)
        {
            _logger.LogInformation($"Executing {nameof(AddOrUpdateAsync)} method...");
            try
            {
                LaneInRFIDTransactionLog? entity;
                if (id.HasValue)
                {
                    var oldEntity = await _repository.GetByIdAsync(id.Value);
                    if (oldEntity != null)
                    {
                        entity = _mapper.Map(input, oldEntity);
                        await _repository.UpdateAsync(entity);

                        return ValidationResult.Success(true);
                    }
                }

                entity = _mapper.Map<LaneInRFIDTransactionLog>(input);
                await _repository.AddAsync(entity);

                return ValidationResult.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(AddOrUpdateAsync)} method. Error: {ex.Message}");
                throw;
            }
        }
    }
}
