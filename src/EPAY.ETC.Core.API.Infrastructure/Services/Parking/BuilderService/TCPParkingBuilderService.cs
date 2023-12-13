using EPAY.ETC.Core.API.Core.Interfaces.Services.Parking.ParkingBuilder;
using EPAY.ETC.Core.API.Core.Models.Enum;
using EPAY.ETC.Core.API.Core.Models.FeeVehicleCategories;
using EPAY.ETC.Core.API.Infrastructure.Models.Configs;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.VehicleFee;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EPAY.ETC.Core.API.Infrastructure.Services.Parking.BuilderService
{
    public class TCPParkingBuilderService : IParkingBuilderService
    {
        private readonly ILogger<TCPParkingBuilderService> _logger;
        private readonly IOptions<ConfigDetails> _options;

        public TCPParkingBuilderService(ILogger<TCPParkingBuilderService> logger, IOptions<ConfigDetails> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public bool IsSupported(string locationId)
        {
            return locationId.Equals("TIA_TCP");
        }

        public (ParkingChargeTypeEnum ParkingChargeType, int DeltaTInSeconds) ParkingProcessing(ParkingRequestModel parking, FeeVehicleCategoryModel? feeVehicleCategory)
        {
            int deltaT = 0;
            try
            {
                _logger.LogInformation($"Executing {nameof(ParkingProcessing)} method...");

                var parkingConfig = _options.Value.ParkingConfigs?.FirstOrDefault(x => x.ParkingLocationId.Equals(parking.LocationId) && x.ParkingFeesApplied == YesNoEnum.Yes);
                deltaT = parkingConfig?.DeltaTInSeconds ?? 0;

                if (
                    parkingConfig != null
                    && _options.Value.ParkingLaneConfigs != null
                    && _options.Value.ParkingLaneConfigs.Any(x =>
                        x.ParkingLocationId == parkingConfig.ParkingLocationId
                        && (
                            x.LaneId.Equals(parking.LaneInId)
                            || x.LaneId.Equals(parking.LaneOutId)
                        )
                        && x.ParkingPaidStatus == PaidStatusEnum.Paid
                    )
                )
                {
                    if (feeVehicleCategory != null && feeVehicleCategory.IsTCPVehicle == true)
                    {
                        return (ParkingChargeTypeEnum.Block0, deltaT);
                    }

                    return (ParkingChargeTypeEnum.Free, deltaT);
                }

                return (ParkingChargeTypeEnum.Charged, deltaT);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(ParkingProcessing)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");

                return (ParkingChargeTypeEnum.Charged, deltaT);
            }
        }
    }
}
