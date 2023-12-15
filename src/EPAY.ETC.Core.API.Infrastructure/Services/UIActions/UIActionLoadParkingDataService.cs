using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.Models.Constants;
using EPAY.ETC.Core.Models.Devices;
using EPAY.ETC.Core.Models.Fees;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;

namespace EPAY.ETC.Core.API.Infrastructure.Services.UIActions
{
    public class UIActionLoadParkingDataService : IUIActionLoadParkingDataService
    {
        private readonly ILogger<UIActionLoadParkingDataService> _logger;
        private readonly IDatabase _redisDB;
        private readonly IServer _server;

        public UIActionLoadParkingDataService(ILogger<UIActionLoadParkingDataService> logger, IDatabase redisDB, IServer server)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _redisDB = redisDB ?? throw new ArgumentNullException(nameof(redisDB));
            _server = server ?? throw new ArgumentNullException(nameof(server));
        }

        public ParkingModel? LoadParkingData(string? rfid, string? plateNumber)
        {
            ParkingModel? parking = null;

            try
            {
                var parkingCamKeys = !string.IsNullOrEmpty(plateNumber) ? _server.Keys(0, $"{RedisConstant.PARKING_KEY}*{plateNumber}*", int.MaxValue).ToList() : null;
                var parkingRFIDKeys = !string.IsNullOrEmpty(rfid) ? _server.Keys(0, $"{RedisConstant.PARKING_KEY}*{rfid}*", int.MaxValue).ToList() : null;

                // Load parking from RFID
                if (parkingRFIDKeys != null && parkingRFIDKeys.Any())
                {
                    foreach (var key in parkingRFIDKeys)
                    {
                        var parkingRFIDRedisValue = _redisDB.StringGet(key.ToString()).ToString();
                        if (!string.IsNullOrEmpty(parkingRFIDRedisValue))
                        {
                            // Deserialize RFID data
                            ParkingRFIDDataModel? rfidData = JsonSerializer.Deserialize<ParkingRFIDDataModel>(parkingRFIDRedisValue);
                            if (rfidData != null)
                            {
                                if (parking == null)
                                    parking = new ParkingModel();

                                var epochTime = DateTimeOffset.FromUnixTimeMilliseconds(rfidData.Epoch).ToUnixTimeSeconds();

                                if (key.ToString().StartsWith(RedisConstant.RFIDInKey(rfid ?? string.Empty, true)))
                                {
                                    if (parking.InEpoch == 0 || parking.InEpoch > epochTime)
                                        parking.InEpoch = epochTime;
                                    parking.RFID = rfidData.TagId;
                                    parking.PlateNumber = rfidData.VehicleInfo?.PlateNumber;
                                    parking.LaneInId = rfidData.LaneId;
                                    parking.LocationId = rfidData.ParkingLocationId;
                                }
                                else
                                {
                                    if (parking.OutEpoch == 0 || parking.OutEpoch < epochTime)
                                        parking.OutEpoch = epochTime;
                                    parking.RFID = rfidData.TagId;
                                    parking.PlateNumber = rfidData.VehicleInfo?.PlateNumber;
                                    parking.LaneOutId = rfidData.LaneId;
                                    parking.LocationId = rfidData.ParkingLocationId;
                                }
                            }
                        }
                    }
                }

                // Load parking from Camera
                if (parkingCamKeys != null && parkingCamKeys.Any())
                {
                    foreach (var key in parkingCamKeys)
                    {
                        var parkingCamRedisValue = _redisDB.StringGet(key.ToString()).ToString();
                        if (!string.IsNullOrEmpty(parkingCamRedisValue))
                        {
                            // Deserialize Cam data
                            ParkingANPRCameraDataModel? parkingCamData = JsonSerializer.Deserialize<ParkingANPRCameraDataModel>(parkingCamRedisValue);
                            if (parkingCamData != null)
                            {
                                if (parking == null)
                                    parking = new ParkingModel();

                                if (key.ToString().StartsWith(RedisConstant.CameraInKey(plateNumber ?? string.Empty, true)))
                                {
                                    if (parking.InEpoch == 0 || parking.InEpoch > parkingCamData.Epoch)
                                        parking.InEpoch = parkingCamData.Epoch;
                                    if (string.IsNullOrEmpty(parking.PlateNumber))
                                        parking.PlateNumber = parkingCamData.VehicleInfo?.PlateNumber;
                                    if (string.IsNullOrEmpty(parking.InPlateNumberPhotoUrl))
                                        parking.InPlateNumberPhotoUrl = !string.IsNullOrEmpty(parkingCamData.VehicleInfo?.PlateNumberPhotoUrl) ? parkingCamData.VehicleInfo?.PlateNumberPhotoUrl : parkingCamData.VehicleInfo?.PlateNumberRearPhotoUrl;
                                    if (string.IsNullOrEmpty(parking.InVehiclePhotoUrl))
                                        parking.InVehiclePhotoUrl = !string.IsNullOrEmpty(parkingCamData.VehicleInfo?.VehiclePhotoUrl) ? parkingCamData.VehicleInfo?.VehiclePhotoUrl : parkingCamData.VehicleInfo?.VehicleRearPhotoUrl;

                                    parking.LaneInId = parkingCamData.LaneId;
                                    parking.LocationId = parkingCamData.ParkingLocationId;
                                }
                                else
                                {
                                    if (parking.OutEpoch == 0 || parking.OutEpoch < parkingCamData.Epoch)
                                        parking.OutEpoch = parkingCamData.Epoch;
                                    if (string.IsNullOrEmpty(parking.PlateNumber))
                                        parking.PlateNumber = parkingCamData.VehicleInfo?.PlateNumber;
                                    if (string.IsNullOrEmpty(parking.OutPlateNumberPhotoUrl))
                                        parking.OutPlateNumberPhotoUrl = !string.IsNullOrEmpty(parkingCamData.VehicleInfo?.PlateNumberPhotoUrl) ? parkingCamData.VehicleInfo?.PlateNumberPhotoUrl : parkingCamData.VehicleInfo?.PlateNumberRearPhotoUrl;
                                    if (string.IsNullOrEmpty(parking.OutVehiclePhotoUrl))
                                        parking.OutVehiclePhotoUrl = !string.IsNullOrEmpty(parkingCamData.VehicleInfo?.VehiclePhotoUrl) ? parkingCamData.VehicleInfo?.VehiclePhotoUrl : parkingCamData.VehicleInfo?.VehicleRearPhotoUrl;

                                    parking.LaneInId = parkingCamData.LaneId;
                                    parking.LocationId = parkingCamData.ParkingLocationId;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                _logger.LogError($"An error occurred when calling {nameof(LoadParkingData)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
            }

            return parking;
        }
    }
}
