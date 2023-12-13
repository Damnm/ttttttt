using EPAY.ETC.Core.API.Core.Models.Enum;
using EPAY.ETC.Core.API.Core.Models.FeeVehicleCategories;
using EPAY.ETC.Core.Models.VehicleFee;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.Parking.ParkingBuilder
{
    public interface IParkingBuilderService
    {
        bool IsSupported(string locationId);
        (ParkingChargeTypeEnum ParkingChargeType, int DeltaTInSeconds) ParkingProcessing(ParkingRequestModel parking, FeeVehicleCategoryModel? feeVehicleCategory);
    }
}
