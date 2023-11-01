using AutoMapper;
using EPAY.ETC.Core.API.Core.Interfaces.Services.PaymentStatus;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PaymentStatus;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Fees.PaymentStatusHistory;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using PaymentStatusModel = EPAY.ETC.Core.API.Core.Models.PaymentStatus.PaymentStatusModel;

namespace EPAY.ETC.Core.API.Infrastructure.Services.PaymentStatus
{
    public class PaymentStatusService : IPaymentStatusService
    {

        #region Variables 
        private readonly ILogger<PaymentStatusService> _logger;
        private readonly IPaymentStatusRepository _repository;
        private readonly IMapper _mapper;
        #endregion
        #region Constructor
        public PaymentStatusService(ILogger<PaymentStatusService> logger, IPaymentStatusRepository fusionRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = fusionRepository ?? throw new ArgumentNullException(nameof(fusionRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        #endregion
        #region AddAsync
        public async Task<ValidationResult<PaymentStatusModel>> AddAsync(PaymentStatusAddRequestModel input)
        {
            _logger.LogInformation($"Executing {nameof(AddAsync)} method...");
            try
            {
                var entity = _mapper.Map<PaymentStatusModel>(input) ?? new PaymentStatusModel();
                if(entity.Id == Guid.Empty)
                    entity.Id = Guid.NewGuid();

                entity.CreatedDate = DateTime.Now;

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
        public async Task<ValidationResult<Core.Models.PaymentStatus.PaymentStatusModel>> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (result == null)
                {
                    return ValidationResult.Failed<Core.Models.PaymentStatus.PaymentStatusModel>(null, new List<ValidationError>()
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
        public async Task<ValidationResult<PaymentStatusModel?>> RemoveAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(RemoveAsync)} method...");
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (result == null)
                {
                    return ValidationResult.Failed<PaymentStatusModel?>(new List<ValidationError>()
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
        public async Task<ValidationResult<Core.Models.PaymentStatus.PaymentStatusModel>> UpdateAsync(Guid id, PaymentStatusUpdateRequestModel request)
        {
            _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");
            try
            {
                var oldRecord = await _repository.GetByIdAsync(id);
                if (oldRecord == null)
                {
                    return ValidationResult.Failed<Core.Models.PaymentStatus.PaymentStatusModel>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
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

        #region GetPaymentStatusHistoryAsync
        public async Task<ValidationResult<List<PaymentStatusHistoryModel>>> GetPaymentStatusHistoryAsync(Guid paymentId)
        {
            _logger.LogInformation($"Executing {nameof(GetPaymentStatusHistoryAsync)} method...");
            try
            {
                var exp = (Expression<Func<PaymentStatusModel, bool>>)((s) => s.PaymentId == paymentId );

                var res = new List<PaymentStatusHistoryModel>();
                var result = await _repository.GetPaymentStatusHistoryAsync((Expression<Func<PaymentStatusModel, bool>>)((s) => s.PaymentId == paymentId && s.Status == PaymentStatusEnum.Failed));
                if (result == null)
                {
                    return ValidationResult.Failed<List<PaymentStatusHistoryModel>>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }

                var items = result.ToList();
                res = _mapper.Map<List<PaymentStatusModel>, List<PaymentStatusHistoryModel>>(items);
               
               return ValidationResult.Success(res);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(GetPaymentStatusHistoryAsync)} method. Error: {ex.Message}");
                throw;
            }
        }
        #endregion
    }
}
