using AutoMapper;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Barcode;
using EPAY.ETC.Core.API.Core.Models.Barcode;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Barcode;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Services.Barcode
{
    public class BarcodeService : IBarcodeService
    {
        #region Variables 
        private readonly ILogger<BarcodeService> _logger;
        private readonly IBarcodeRepository _barcodeRepository;
        private readonly IMapper _mapper;
        #endregion
        #region Constructor
        public BarcodeService(ILogger<BarcodeService> logger, IBarcodeRepository barcodeRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _barcodeRepository = barcodeRepository ?? throw new ArgumentNullException(nameof(barcodeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        #endregion
        #region AddAsync
        public async Task<ValidationResult<BarcodeModel>> AddAsync(BarcodeAddOrUpdateRequestModel input)
        {
            _logger.LogInformation($"Executing {nameof(AddAsync)} method...");
            try
            {
                var existingRecords = await GetExistingRecordAsync(input);
                if (existingRecords)
                {
                    return ValidationResult.Failed<BarcodeModel>(new List<ValidationError>()
                    {
                        new ValidationError("Gía trị đã có trên hệ thống", ValidationError.Conflict.Code)
                    });
                }
                var entity = _mapper.Map<BarcodeModel>(input);
                entity.Id = Guid.NewGuid();
                entity.CreatedDate = DateTime.Now;

                var result = await _barcodeRepository.AddAsync(entity);
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
        public async Task<ValidationResult<BarcodeModel>> GetByIdAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(GetByIdAsync)} method...");
            try
            {
                var result = await _barcodeRepository.GetByIdAsync(id);
                if (result == null)
                {
                    return ValidationResult.Failed<BarcodeModel>(null, new List<ValidationError>()
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

        public async Task<ValidationResult<List<BarcodeModel>>> GetListAsync(Expression<Func<BarcodeModel, bool>>? expression)
        {
            _logger.LogInformation($"Executing {nameof(GetListAsync)} method...");
            try
            {
                var result = await _barcodeRepository.GetAllAsync(expression);
                if (result == null)
                {
                    return ValidationResult.Failed<List<BarcodeModel>>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }

                return ValidationResult.Success(result.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(GetListAsync)} method. Error: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region RemoveAsync
        public async Task<ValidationResult<BarcodeModel>> RemoveAsync(Guid id)
        {
            _logger.LogInformation($"Executing {nameof(RemoveAsync)} method...");
            try
            {
                var result = await _barcodeRepository.GetByIdAsync(id);
                if (result == null)
                {
                    return ValidationResult.Failed<BarcodeModel>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }

                await _barcodeRepository.RemoveAsync(result);

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
        public async Task<ValidationResult<BarcodeModel>> UpdateAsync(Guid id, BarcodeAddOrUpdateRequestModel request)
        {
            _logger.LogInformation($"Executing {nameof(UpdateAsync)} method...");
            try
            {
                var oldRecord = await _barcodeRepository.GetByIdAsync(id);
                if (oldRecord == null)
                {
                    return ValidationResult.Failed<BarcodeModel>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }
                if (oldRecord.Id != id)
                {
                    return ValidationResult.Failed<BarcodeModel>(new List<ValidationError>()
                    {
                        new ValidationError("Giá trị đã có trên hệ thống", ValidationError.Conflict.Code)
                    });
                }
                _mapper.Map(request, oldRecord);

                await _barcodeRepository.UpdateAsync(oldRecord);
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
        private async Task<bool> GetExistingRecordAsync(BarcodeAddOrUpdateRequestModel input)
        {
            Expression<Func<BarcodeModel, bool>> expression = s =>
                s.ActionCode == input.ActionCode;

            var result = await _barcodeRepository.GetAllAsync(expression);

            return result.Any();
        }
        #endregion
    }
}
