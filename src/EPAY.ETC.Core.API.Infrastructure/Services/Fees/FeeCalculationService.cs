using EPAY.ETC.Core.API.Core.Interfaces.Services.Fees;
using EPAY.ETC.Core.API.Core.Utils;
using EPAY.ETC.Core.API.Infrastructure.Common.Utils;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.CustomVehicleTypes;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.FeeVehicleCategories;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TimeBlockFees;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Utils;
using EPAY.ETC.Core.Models.Validation;
using EPAY.ETC.Core.Models.VehicleFee;
using Microsoft.Extensions.Logging;
using FeeTypeEnum = EPAY.ETC.Core.API.Core.Models.Enum.FeeTypeEnum;

namespace EPAY.ETC.Core.API.Infrastructure.Services.Fees
{
    public class FeeCalculationService : IFeeCalculationService
    {
        private readonly ILogger<FeeCalculationService> _logger;
        private readonly IFeeVehicleCategoryRepository _feeVehicleCategoryRepository;
        private readonly ITimeBlockFeeRepository _timeBlockFeeRepository;
        private readonly ITimeBlockFeeFormulaRepository _feeFormulaRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ICustomVehicleTypeRepository _customVehicleTypeRepository;

        public FeeCalculationService(ILogger<FeeCalculationService> logger,
                                     IFeeVehicleCategoryRepository feeVehicleCategoryRepository,
                                     ITimeBlockFeeRepository timeBlockFeeRepository,
                                     ITimeBlockFeeFormulaRepository feeFormulaRepository,
                                     IVehicleRepository vehicleRepository,
                                     ICustomVehicleTypeRepository customVehicleTypeRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _feeVehicleCategoryRepository = feeVehicleCategoryRepository ?? throw new ArgumentNullException(nameof(feeVehicleCategoryRepository));
            _timeBlockFeeRepository = timeBlockFeeRepository ?? throw new ArgumentNullException(nameof(timeBlockFeeRepository));
            _feeFormulaRepository = feeFormulaRepository ?? throw new ArgumentNullException(nameof(feeFormulaRepository));
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
            _customVehicleTypeRepository = customVehicleTypeRepository ?? throw new ArgumentNullException(nameof(customVehicleTypeRepository));
        }

        // TODO: Do check vehicle in BOO?
        public async Task<ValidationResult<VehicleFeeModel>> CalculateFeeAsync(string? rfid, string? plateNumber, long checkInDateEpoch, long checkOutDateEpoch)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(CalculateFeeAsync)} method...");

                // Init variable using
                long duration = checkOutDateEpoch - checkInDateEpoch;
                DateTime checkOutDateTime = checkOutDateEpoch.ToSpecificDateTime();
                FeeTypeEnum feeType = FeeTypeEnum.TimeBlock;
                CustomVehicleTypeEnum customVehicleType = CustomVehicleTypeEnum.Type1;

                // Init result return
                var result = new VehicleFeeModel()
                {
                    Fee = new FeeModel()
                    {
                        Amount = 0,
                        DurationTime = duration,
                        BlockNo = 0
                    }
                };

                // Get vehicle already defined
                var feeVehicleCategories = await _feeVehicleCategoryRepository.GetAllAsync(x =>
                    (
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
                    )
                    && x.ValidFrom >= checkOutDateTime && (x.ValidTo == null || x.ValidTo >= checkOutDateTime)
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
                        VehicleCategoryName = feeVehicleCategory.VehicleCategory?.VehicleCategoryName,
                        VehicleGroupId = feeVehicleCategory.VehicleGroupId,
                        VehicleGroupName = feeVehicleCategory.VehicleGroup?.GroupName
                    };

                    // Set fee type if exists
                    feeType = feeVehicleCategory.FeeType?.FeeName ?? FeeTypeEnum.TimeBlock;
                }
                else if (!string.IsNullOrEmpty(rfid))
                {
                    var vehicles = await _vehicleRepository.GetAllAsync(x => x.RFID == rfid);

                    if (vehicles.Any())
                    {
                        var vehicle = vehicles.FirstOrDefault()!;

                        customVehicleType = VehicleTypeConverter.ConvertVehicleType(vehicle.Seat ?? 0, vehicle.Weight);

                        result.Vehicle = new VehicleModel()
                        {
                            PlateNumber = vehicle.PlateNumber,
                            RFID = vehicle.RFID,
                            Make = vehicle.Make,
                            Model = vehicle.Model
                        };
                    }
                }

                // Load fee fomular
                var timeBlockFeeFormulas = await _feeFormulaRepository.GetAllAsync(x => x.CustomVehicleType != null && x.CustomVehicleType.Name == customVehicleType);
                var timeBlockFees = await _timeBlockFeeRepository.GetAllAsync(x => x.CustomVehicleType != null && x.CustomVehicleType.Name == customVehicleType);

                // Calculate fee and get block
                var feeCalculation = FeeCalculationUtil.FeeCalculation(timeBlockFees?.ToList(), timeBlockFeeFormulas.FirstOrDefault(), duration);

                result.Fee.BlockNo = feeCalculation.Block;
                result.Fee.DurationTime = feeCalculation.Duration;

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

                        result.Fee.Amount = feeCalculation.Amount;
                        break;
                }

                // Set CustomVehicleType info
                var customVehicleTypes = await _customVehicleTypeRepository.GetAllAsync(x => x.Name == customVehicleType);

                if (result.Vehicle == null)
                    result.Vehicle = new VehicleModel();
                result.Vehicle.CustomVehicleTypeId = customVehicleTypes?.Select(x => x.Id).FirstOrDefault();
                result.Vehicle.CustomVehicleTypeCode = customVehicleType.ToString();
                result.Vehicle.CustomVehicleTypeName = customVehicleType.ToEnumMemberAttrValue();

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
                DateTime checkOutDateTime = checkOutDateEpoch.ToSpecificDateTime();
                FeeTypeEnum feeType = FeeTypeEnum.TimeBlock;

                // Init result return
                var result = new VehicleFeeModel()
                {
                    Fee = new FeeModel()
                    {
                        Amount = 0,
                    }
                };

                // Get vehicle already defined
                var feeVehicleCategories = await _feeVehicleCategoryRepository.GetAllAsync(x =>
                    x.PlateNumber != null
                    && !string.IsNullOrEmpty(plateNumber)
                    && x.PlateNumber.Equals(plateNumber)
                    && x.ValidFrom >= checkOutDateTime && (x.ValidTo == null || x.ValidTo >= checkOutDateTime)
                );
                var feeVehicleCategory = feeVehicleCategories.OrderBy(x => x.PlateNumber).FirstOrDefault();

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
                        VehicleCategoryName = feeVehicleCategory.VehicleCategory?.VehicleCategoryName,
                        VehicleGroupId = feeVehicleCategory.VehicleGroupId,
                        VehicleGroupName = feeVehicleCategory.VehicleGroup?.GroupName
                    };

                    // Set fee type if exists
                    feeType = feeVehicleCategory.FeeType?.FeeName ?? FeeTypeEnum.TimeBlock;
                }

                // Load fee fomular
                var timeBlockFeeFormulas = await _feeFormulaRepository.GetAllAsync(x => x.CustomVehicleType != null && x.CustomVehicleType.Name == customVehicleType);
                var timeBlockFees = await _timeBlockFeeRepository.GetAllAsync(x => x.CustomVehicleType != null && x.CustomVehicleType.Name == customVehicleType);

                var feeCalculation = FeeCalculationUtil.FeeCalculation(timeBlockFees?.ToList(), timeBlockFeeFormulas.FirstOrDefault(), duration);

                result.Fee.BlockNo = feeCalculation.Block;
                result.Fee.DurationTime = feeCalculation.Duration;

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
                        Guid? customVehilceTypeId = null;

                        if (feeVehicleCategory?.CustomVehicleType != null)
                        {
                            customVehicleType = feeVehicleCategory.CustomVehicleType.Name;
                            customVehilceTypeId = feeVehicleCategory.CustomVehicleType.Id;
                        }

                        if (customVehilceTypeId == null)
                        {
                            var customVehicleTypes = await _customVehicleTypeRepository.GetAllAsync(x => x.Name == customVehicleType);
                            customVehilceTypeId = customVehicleTypes?.Select(x => x.Id).FirstOrDefault();
                        }

                        result.Fee.Amount = feeCalculation.Amount;
                        result.Vehicle = new VehicleModel()
                        {
                            PlateNumber = plateNumber,
                            CustomVehicleTypeId = customVehilceTypeId,
                            CustomVehicleTypeCode = customVehicleType?.ToString(),
                            CustomVehicleTypeName = customVehicleType?.ToEnumMemberAttrValue(),
                        };
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
    }
}
