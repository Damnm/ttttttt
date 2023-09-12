using EPAY.ETC.Core.API.Core.Validation;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.VehicleFee;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.Fees
{
    public interface IFeeCalculationService
    {
        Task<ValidationResult<VehicleFeeModel>> CalculateFeeAsync(string? rfid, string? plateNumber, long checkInDateEpoch, long checkOutDateEpoch);
        Task<ValidationResult<VehicleFeeModel>> CalculateFeeAsync(string? plateNumber, CustomVehicleTypeEnum? customVehicleType, long checkInDateEpoch, long checkOutDateEpoch);
    }
}
