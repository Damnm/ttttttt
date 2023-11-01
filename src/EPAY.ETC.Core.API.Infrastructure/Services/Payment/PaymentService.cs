using AutoMapper;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Payment;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Payment;
using EPAY.ETC.Core.Models.Constants;
using EPAY.ETC.Core.Models.Fees.PaidVehicleHistory;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using PaymentModel = EPAY.ETC.Core.API.Core.Models.Payment.PaymentModel;

namespace EPAY.ETC.Core.API.Infrastructure.Services.Payment
{
    public class PaymentService : IPaymentService
    {

        #region Variables
        private readonly ILogger<PaymentService> _logger;
        private readonly IPaymentRepository _repository;
        private readonly IMapper _mapper;
        #endregion
        #region Constructor
        public PaymentService(ILogger<PaymentService> logger, IPaymentRepository fusionRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = fusionRepository ?? throw new ArgumentNullException(nameof(fusionRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        #endregion
        #region AddAsync
        public async Task<ValidationResult<PaymentModel>> AddAsync(PaymentAddOrUpdateRequestModel input)
        {
            _logger.LogInformation($"Executing {nameof(AddAsync)} method...");
            try
            {
                var existingRecords = await GetExistingRecordAsync(input);
                if (existingRecords)
                {
                    return ValidationResult.Failed<PaymentModel>(new List<ValidationError>()
                    {
                        new ValidationError("Gía trị đã có trên hệ thống", ValidationError.Conflict.Code)
                    });
                }
                var entity = _mapper.Map<PaymentModel>(input);

                var result = await _repository.AddAsync(entity);
                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(AddAsync)} method. Error: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region GetIdAsync
        public async Task<ValidationResult<PaymentModel>> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (result == null)
                {
                    return ValidationResult.Failed<PaymentModel>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }

                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(GetByIdAsync)} method. Error: {ex.Message}");
                throw;
            }
        }

       
        #endregion

        #region RemoveAsync
        public async Task<ValidationResult<PaymentModel?>> RemoveAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(RemoveAsync)} method...");
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (result == null)
                {
                    return ValidationResult.Failed<PaymentModel?>(new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }

                await _repository.RemoveAsync(result);

                return ValidationResult.Success(result = null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(RemoveAsync)} method. Error: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region UpdateAsync
        public async Task<ValidationResult<PaymentModel>> UpdateAsync(Guid id, PaymentAddOrUpdateRequestModel request)
        {
            _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");
            try
            {
                var existingRecords = await GetExistingRecordAsync(request, id);
                if (existingRecords)
                {
                    return ValidationResult.Failed<PaymentModel>(new List<ValidationError>()
                    {
                        new ValidationError("Gía trị đã có trên hệ thống", ValidationError.Conflict.Code)
                    });
                }

                var oldRecord = await _repository.GetByIdAsync(id);
                if (oldRecord == null)
                {
                    return ValidationResult.Failed<PaymentModel>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }
                if (oldRecord.Id != id)
                {
                    return ValidationResult.Failed<PaymentModel>(new List<ValidationError>()
                    {
                        new ValidationError("Giá trị đã có trên hệ thống", ValidationError.Conflict.Code)
                    });
                }
                _mapper.Map(request, oldRecord);

                await _repository.UpdateAsync(oldRecord);
                return ValidationResult.Success(oldRecord);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(UpdateAsync)} method. Error: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Private method
        private async Task<bool> GetExistingRecordAsync(PaymentAddOrUpdateRequestModel input, Guid? id = null)
        {
            Expression<Func<PaymentModel, bool>> expression = s =>
                s.FeeId == input.FeeId;

            var result = await _repository.GetAllAsync(expression);

            return result.Any(x => !x.Id.Equals(id));
        }
        #endregion

        #region GetPaidVehicleHistoryAsync
        public async Task<ValidationResult<List<PaidVehicleHistoryModel>>> GetPaidVehicleHistoryAsync(string? laneId = null)
        {
            _logger.LogInformation($"Executing {nameof(GetPaidVehicleHistoryAsync)} method...");
            try
            {
                if (string.IsNullOrEmpty(laneId))
                {
                    laneId = Environment.GetEnvironmentVariable(CoreConstant.ENVIRONMENT_LANE_OUT) ?? "1";
                }

                var result = await _repository.GetPaidVehicleHistoryAsync(laneId);
                if (result == null)
                {
                    return ValidationResult.Failed<List<PaidVehicleHistoryModel>>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }

                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(GetPaidVehicleHistoryAsync)} method. Error: {ex.Message}");
                throw;
            }
        }
        #endregion
    }
}
