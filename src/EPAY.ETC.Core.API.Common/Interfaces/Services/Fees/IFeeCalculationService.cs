using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Validation;
using EPAY.ETC.Core.Models.VehicleFee;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.Fees
{
    public interface IFeeCalculationService
    {
        Task<ValidationResult<VehicleFeeModel>> CalculateFeeAsync(string? rfid, string? plateNumber, long checkInDateEpoch, long checkOutDateEpoch, ParkingRequestModel? parking = null);
        Task<ValidationResult<VehicleFeeModel>> CalculateFeeAsync(string? plateNumber, CustomVehicleTypeEnum? customVehicleType, long checkInDateEpoch, long checkOutDateEpoch, ParkingRequestModel? parking = null);
    }
}
