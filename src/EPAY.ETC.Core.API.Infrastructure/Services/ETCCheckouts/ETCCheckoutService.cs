using AutoMapper;
using EPAY.ETC.Core.API.Core.Interfaces.Services.ETCCheckouts;
using EPAY.ETC.Core.API.Core.Models.ETCCheckOuts;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ETCCheckouts;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Services.ETCCheckouts
{
    public class ETCCheckoutService : IETCCheckoutService
    {
        private readonly ILogger<ETCCheckoutService> _logger;
        private readonly IETCCheckoutRepository _repository;
        private readonly IMapper _mapper;

        public ETCCheckoutService(
            ILogger<ETCCheckoutService> logger, IETCCheckoutRepository repository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper;
        }

        public async Task<ValidationResult<ETCCheckoutResponseModel>> AddAsync(ETCCheckoutAddUpdateRequestModel input)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(AddAsync)} method...");

                bool existRecord = await CheckExistsRecordByPaymentIdAndTransactionId(null, input.PaymentId, input.TransactionId);
                if (existRecord)
                {
                    return ValidationResult.Failed<ETCCheckoutResponseModel>(new List<ValidationError>()
                    {
                        ValidationError.Conflict
                    });
                }

                var entity = _mapper.Map<ETCCheckoutDataModel>(input);

                var result = await _repository.AddAsync(entity);

                return ValidationResult.Success(_mapper.Map<ETCCheckoutResponseModel>(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(AddAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ValidationResult<IEnumerable<ETCCheckoutResponseModel>>> GetAllAsync(Expression<Func<ETCCheckoutDataModel, bool>>? expressison = null)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");

                var result = await _repository.GetAllAsync(expressison);

                return ValidationResult.Success(_mapper.Map<IEnumerable<ETCCheckoutDataModel>, IEnumerable<ETCCheckoutResponseModel>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ValidationResult<ETCCheckoutResponseModel?>> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");

                var result = await _repository.GetByIdAsync(id);

                return ValidationResult.Success(_mapper.Map<ETCCheckoutResponseModel?>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetByIdAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ValidationResult<ETCCheckoutResponseModel?>> RemoveAsync(Guid id)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(RemoveAsync)} method...");

                var oldRecord = await _repository.GetByIdAsync(id);
                if (oldRecord == null)
                {
                    return ValidationResult.Failed<ETCCheckoutResponseModel?>(new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }

                await _repository.RemoveAsync(oldRecord);

                return ValidationResult.Success<ETCCheckoutResponseModel?>(null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(RemoveAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ValidationResult<ETCCheckoutResponseModel>> UpdateAsync(Guid id, ETCCheckoutAddUpdateRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");

                var oldRecord = await _repository.GetByIdAsync(id);
                if (oldRecord == null)
                {
                    return ValidationResult.Failed<ETCCheckoutResponseModel>(new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }

                bool existRecord = await CheckExistsRecordByPaymentIdAndTransactionId(id, request.PaymentId, request.TransactionId);
                if (existRecord)
                {
                    return ValidationResult.Failed<ETCCheckoutResponseModel>(new List<ValidationError>()
                    {
                        ValidationError.Conflict
                    });
                }

                var entity = _mapper.Map<ETCCheckoutDataModel>(request);
                entity.Id = id;

                await _repository.UpdateAsync(entity);

                return ValidationResult.Success(_mapper.Map<ETCCheckoutResponseModel>(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(UpdateAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        #region Private method
        private async Task<bool> CheckExistsRecordByPaymentIdAndTransactionId(Guid? id, Guid paymentId, string transactionId)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(CheckExistsRecordByPaymentIdAndTransactionId)} method...");

                Expression<Func<ETCCheckoutDataModel, bool>> expression = s => (id.HasValue ? s.Id != id : true) && (s.PaymentId == paymentId || s.TransactionId.Equals(transactionId));

                var result = await _repository.GetAllAsync(expression);

                return result.Any();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(CheckExistsRecordByPaymentIdAndTransactionId)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion
    }
}
