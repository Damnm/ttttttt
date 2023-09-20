using EPAY.ETC.Core.API.Core.Extensions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.Fees;
using EPAY.ETC.Core.API.Core.Models.Enum;
using EPAY.ETC.Core.API.Core.Utils;
using EPAY.ETC.Core.API.Infrastructure.Common.Utils;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.FeeVehicleCategories;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TimeBlockFees;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Validation;
using EPAY.ETC.Core.Models.VehicleFee;
using Microsoft.Extensions.Logging;

namespace EPAY.ETC.Core.API.Infrastructure.Services.Fees
{
    public class FeeCalculationService : IFeeCalculationService
    {
        private readonly ILogger<FeeCalculationService> _logger;
        private readonly IFeeVehicleCategoryRepository _feeVehicleCategoryRepository;
        private readonly ITimeBlockFeeRepository _timeBlockFeeRepository;
        private readonly ITimeBlockFeeFormulaRepository _feeFormulaRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public FeeCalculationService(
            ILogger<FeeCalculationService> logger,
            IFeeVehicleCategoryRepository feeVehicleCategoryRepository,
            ITimeBlockFeeRepository timeBlockFeeRepository,
            ITimeBlockFeeFormulaRepository feeFormulaRepository,
            IVehicleRepository vehicleRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _feeVehicleCategoryRepository = feeVehicleCategoryRepository ?? throw new ArgumentNullException(nameof(feeVehicleCategoryRepository));
            _timeBlockFeeRepository = timeBlockFeeRepository ?? throw new ArgumentNullException(nameof(timeBlockFeeRepository));
            _feeFormulaRepository = feeFormulaRepository ?? throw new ArgumentNullException(nameof(feeFormulaRepository));
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        }

        // TODO: Do check vehicle in BOO?
        public async Task<ValidationResult<VehicleFeeModel>> CalculateFeeAsync(string? rfid, string? plateNumber, long checkInDateEpoch, long checkOutDateEpoch)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(CalculateFeeAsync)} method...");

                // Init variable using
                long duration = checkOutDateEpoch - checkInDateEpoch;
                FeeTypeEnum feeType = FeeTypeEnum.TimeBlock;
                CustomVehicleTypeEnum customVehicleType = CustomVehicleTypeEnum.Type1;

                // Init result return
                var result = new VehicleFeeModel()
                {
                    Fee = new FeeModel()
                    {
                        Amount = 0,
                        Duration = (int)duration,
                        Currency = CurrencyEnum.VND.ToString()
                    }
                };

                // Get vehicle already defined
                var feeVehicleCategories = await _feeVehicleCategoryRepository.GetAllAsync(x =>
                    (
                        x.RFID != null
                        && !string.IsNullOrEmpty(rfid)
                        && x.RFID.Equals(rfid)
                    )
                    || (
                        x.PlateNumber != null
                        && !string.IsNullOrEmpty(plateNumber)
                        && x.PlateNumber.Equals(plateNumber)
                    )
                );
                var feeVehicleCategory = feeVehicleCategories.OrderBy(x => x.RFID).ThenBy(x => x.PlateNumber).FirstOrDefault();

                // If exists
                if (feeVehicleCategory != null)
                {
                    // Update result vehicle
                    result.Vehicle = new VehicleModel()
                    {
                        PlateNumber = feeVehicleCategory.PlateNumber,
                        RFID = feeVehicleCategory.RFID,
                        CustomVehicleTypeId = feeVehicleCategory.CustomVehicleTypeId,
                        CustomVehicleTypeCode = feeVehicleCategory.CustomVehicleType?.Name.ToString(),
                        CustomVehicleTypeName = feeVehicleCategory.CustomVehicleType?.Name.ToEnumMemberAttrValue(),
                        VehicleCategoryId = feeVehicleCategory.VehicleCategoryId,
                        VehicleCategoryName = feeVehicleCategory.VehicleCategory?.Name,
                        VehicleGroupId = feeVehicleCategory.VehicleGroupId,
                        VehicleGroupName = feeVehicleCategory.VehicleGroup?.Name
                    };

                    // Set fee type if exists
                    feeType = feeVehicleCategory.FeeType?.Name ?? FeeTypeEnum.TimeBlock;
                }
                else if (!string.IsNullOrEmpty(rfid))
                {
                    var vehicles = await _vehicleRepository.GetAllAsync(x => x.RFID == rfid);

                    if (vehicles.Any())
                    {
                        var vehicle = vehicles.FirstOrDefault()!;

                        customVehicleType = VehicleTypeConverter.ConvertVehicleType(vehicle.Seat ?? 0, vehicle.Weight);
                    }
                }

                // Calculate fee
                switch (feeType)
                {
                    case FeeTypeEnum.Free:
                        result.Fee.Amount = 0;
                        break;

                    case FeeTypeEnum.Fixed:
                        result.Fee.Amount = feeVehicleCategory?.FeeType?.Amount ?? 0;
                        break;

                    // Calculate using TimeBlockFee
                    default:
                        if (feeVehicleCategory?.CustomVehicleType != null)
                            customVehicleType = feeVehicleCategory.CustomVehicleType.Name;
                        var timeBlockFeeFormulas = await _feeFormulaRepository.GetAllAsync(x => x.CustomVehicleType != null && x.CustomVehicleType.Name == customVehicleType);
                        var timeBlockFees = await _timeBlockFeeRepository.GetAllAsync(x => x.CustomVehicleType != null && x.CustomVehicleType.Name == customVehicleType);
                        result.Fee.Amount = FeeCalculationUtil.FeeCalculation(timeBlockFees?.ToList(), timeBlockFeeFormulas.FirstOrDefault(), duration);
                        break;
                }

                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(CalculateFeeAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ValidationResult<VehicleFeeModel>> CalculateFeeAsync(string? plateNumber, CustomVehicleTypeEnum? customVehicleType, long checkInDateEpoch, long checkOutDateEpoch)
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
                        Duration = (int)duration,
                        Currency = CurrencyEnum.VND.ToString()
                    }
                };

                // Calculate using TimeBlockFee
                var timeBlockFeeFormulas = await _feeFormulaRepository.GetAllAsync(x => x.CustomVehicleType != null && x.CustomVehicleType.Name == customVehicleType);
                var timeBlockFees = await _timeBlockFeeRepository.GetAllAsync(x => x.CustomVehicleType != null && x.CustomVehicleType.Name == customVehicleType);
                result.Fee.Amount = FeeCalculationUtil.FeeCalculation(timeBlockFees?.ToList(), timeBlockFeeFormulas.FirstOrDefault(), duration);

                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(CalculateFeeAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
