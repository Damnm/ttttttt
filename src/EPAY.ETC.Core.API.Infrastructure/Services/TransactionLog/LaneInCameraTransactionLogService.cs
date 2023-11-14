using AutoMapper;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Transaction;
using EPAY.ETC.Core.API.Core.Models.TransactionLog;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TransactionLog;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Services.TransactionLog
{
    public class LaneInCameraTransactionLogService : ILaneInCameraTransactionLogService
    {

        #region Variables 
        private readonly ILogger<LaneInCameraTransactionLogService> _logger;
        private readonly ILaneInCameraTransactionLogRepository _repository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public LaneInCameraTransactionLogService(ILogger<LaneInCameraTransactionLogService> logger, ILaneInCameraTransactionLogRepository repository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #endregion
        public async Task<ValidationResult<bool>> UpdateInsertAsync(LaneInCameraTransactionLogRequest input)
        {
            _logger.LogInformation($"Executing {nameof(UpdateInsertAsync)} method...");
            try
            {
                var existRecords = await _repository.GetAllAsync(s => 
                 s.PlateNumber == input.PlateNumber && s.Epoch == input.Epoch);

                var entityMapped = _mapper.Map<LaneInCameraTransactionLog>(input);

                if (existRecords.Any())
                {
                    var item = existRecords.FirstOrDefault();
                    entityMapped.Epoch = item?.Epoch ?? 0;
                    entityMapped.PlateNumber = item?.PlateNumber;

                    await UpdateAsync(entityMapped);
                    return ValidationResult.Success(true);
                }

                await _repository.AddAsync(entityMapped);
                return ValidationResult.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(UpdateInsertAsync)} method. Error: {ex.Message}");
                throw;
            }
        }

        #region UpdateAsync
        public async Task<ValidationResult<LaneInCameraTransactionLog>> UpdateAsync(LaneInCameraTransactionLog entity)
        {
            _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");
            try
            {
               await _repository.UpdateAsync(entity);
               return ValidationResult.Success(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(UpdateAsync)} method. Error: {ex.Message}");
                throw;
            }
        }
        #endregion
    }
}
