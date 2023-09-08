using EPAY.ETC.Core.API.Core.Extensions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Fees;
using EPAY.ETC.Core.API.Core.Models.Enum;
using EPAY.ETC.Core.API.Infrastructure.Common.Utils;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.FeeVehicleCategories;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TimeBlockFees;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.VehicleFee;
using Microsoft.Extensions.Logging;

namespace EPAY.ETC.Core.API.Infrastructure.Services.Fees
{
    public class FeeCalculationService : IFeeCalculationService
    {
        private readonly ILogger<FeeCalculationService> _logger;
        private readonly IFeeVehicleCategoryRepository _feeVehicleCategoryRepository;
        private readonly ITimeBlockFeeRepository _timeBlockFeeRepository;

        public FeeCalculationService(
            ILogger<FeeCalculationService> logger,
            IFeeVehicleCategoryRepository feeVehicleCategoryRepository,
            ITimeBlockFeeRepository timeBlockFeeRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _feeVehicleCategoryRepository = feeVehicleCategoryRepository ?? throw new ArgumentNullException(nameof(feeVehicleCategoryRepository));
            _timeBlockFeeRepository = timeBlockFeeRepository ?? throw new ArgumentNullException(nameof(timeBlockFeeRepository));
        }

        public async Task<VehicleFeeModel> CalculateFeeAsync(string RFID, string? plateNumber, long checkInDateEpoch, long checkOutDateEpoch)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(CalculateFeeAsync)} method...");

                // Init variable using
                int duration = (int)(checkOutDateEpoch - checkInDateEpoch);
                bool isCalculateByTimeBlock = true;
                CustomVehicleTypeEnum customVehicleType = CustomVehicleTypeEnum.Type1;

                // Init result return
                var result = new VehicleFeeModel()
                {
                    Fee = new FeeModel()
                    {
                        Amount = 0,
                        Duration = (int)duration
                    }
                };

                // Get vehicle already defined
                var feeVehicleCategory = (await _feeVehicleCategoryRepository.GetAllAsync(x => x.RFID != null && x.RFID.Equals(RFID) || x.PlateNumber != null && x.PlateNumber.Equals(plateNumber))).OrderBy(x => x.RFID).FirstOrDefault();

                // If exists
                if (feeVehicleCategory != null)
                {
                    result.Vehicle = new VehicleModel()
                    {
                        PlateNumber = feeVehicleCategory.PlateNumber,
                        RFID = feeVehicleCategory.RFID,
                        CustomVehicleTypeCode = feeVehicleCategory.CustomVehicleType?.Name.ToString(),
                        CustomVehicleTypeName = feeVehicleCategory.CustomVehicleType?.Name.ToEnumMemberAttrValue(),
                        VehicleCategory = feeVehicleCategory.VehicleCategory?.Name,
                        VehicleGroup = feeVehicleCategory.VehicleGroup?.Name
                    };

                    // If exists fee type
                    if (feeVehicleCategory.FeeType != null)
                    {
                        // Return if FeeType = Fixed or Free
                        if (feeVehicleCategory.FeeType.Name == FeeTypeEnum.Fixed || feeVehicleCategory.FeeType.Name == FeeTypeEnum.Free)
                        {
                            result.Fee.Amount = feeVehicleCategory.FeeType.Amount ?? 0;
                            isCalculateByTimeBlock = false;
                        }
                        else if (feeVehicleCategory.FeeType.Name == FeeTypeEnum.TimeBlock)
                            // Set vehicleType must be calculate
                            customVehicleType = feeVehicleCategory.CustomVehicleType?.Name ?? CustomVehicleTypeEnum.Type1;
                        else
                            isCalculateByTimeBlock = false;
                    }
                }

                // Calculate using TimeBlockFee
                if (isCalculateByTimeBlock)
                {
                    var timeBlockFees = await _timeBlockFeeRepository.GetAllAsync(x => x.CustomVehicleType != null && x.CustomVehicleType.Name == customVehicleType);
                    result.Fee.Amount = FeeCalculationUtil.FeeCalculation(timeBlockFees?.ToList(), duration);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(CalculateFeeAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<VehicleFeeModel> CalculateFeeAsync(string plateNumber, CustomVehicleTypeEnum customVehicleType, long checkInDateEpoch, long checkOutDateEpoch)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(CalculateFeeAsync)} method...");

                // Init variable using
                long duration = checkOutDateEpoch - checkInDateEpoch;

                // Init result return
                var result = new VehicleFeeModel()
                {
                    Fee = new FeeModel()
                    {
                        Amount = 0,
                        Duration = (int)duration
                    }
                };

                // Calculate using TimeBlockFee
                var timeBlockFees = await _timeBlockFeeRepository.GetAllAsync(x => x.CustomVehicleType != null && x.CustomVehicleType.Name == customVehicleType);
                result.Fee.Amount = FeeCalculationUtil.FeeCalculation(timeBlockFees?.ToList(), duration);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(CalculateFeeAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
