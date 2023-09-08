using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.VehicleFee;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.Fees
{
    public interface IFeeCalculationService
    {
        Task<VehicleFeeModel> CalculateFeeAsync(string RFID, string? plateNumber, long checkInDateEpoch, long checkOutDateEpoch);
        Task<VehicleFeeModel> CalculateFeeAsync(string plateNumber, CustomVehicleTypeEnum customVehicleType, long checkInDateEpoch, long checkOutDateEpoch);
    }
}
