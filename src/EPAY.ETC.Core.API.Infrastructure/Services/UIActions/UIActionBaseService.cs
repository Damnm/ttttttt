using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.API.Infrastructure.Common.Extensions;
using EPAY.ETC.Core.Models.Constants;
using EPAY.ETC.Core.Models.Devices;
using EPAY.ETC.Core.Models.Fees;
using EPAY.ETC.Core.Models.UI;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;

namespace EPAY.ETC.Core.API.Infrastructure.Services.UIActions
{
    public abstract class UIActionBaseService
    {
        private readonly ILogger<UIActionBaseService> _logger;
        public readonly IDatabase _redisDB;
        public readonly IServer _redisServer;

        public UIActionBaseService(ILogger<UIActionBaseService> logger,
                               IDatabase redisDB,
                               IServer server)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _redisDB = redisDB ?? throw new ArgumentNullException(nameof(redisDB));
            _redisServer = server ?? throw new ArgumentNullException(nameof(server));
        }

        #region Private method
        public List<WaitingVehicleModel> GetWaitingVehicles()
        {
            _logger.LogInformation($"Executing {nameof(GetWaitingVehicles)} method...");

            var fusionObjects = HashGetList<FusionModel>(null, RedisConstant.SORTED_SET_FUSION_OUT);
            List<WaitingVehicleModel> waitingVehicles = new List<WaitingVehicleModel>();
            if (fusionObjects != null && fusionObjects.Any())
            {
                foreach (var fusion in fusionObjects)
                {
                    string? plateNumber = fusion.ANPRCam1;
                    long? inEpoch = null;

                    if (!string.IsNullOrEmpty(fusion.ANPRCam1))
                    {
                        var camData = _redisDB.StringGet(RedisConstant.CameraInKey(fusion.ANPRCam1)).ToString();
                        if (!string.IsNullOrEmpty(camData))
                        {
                            var anprCamValue = JsonSerializer.Deserialize<ANPRCameraModel>(camData);
                            inEpoch = anprCamValue?.CheckpointTimeEpoch;
                        }
                    }

                    if (!string.IsNullOrEmpty(fusion.RFID))
                    {
                        var rfidIn = _redisDB.StringGet(RedisConstant.RFIDInKey(fusion.RFID)).ToString();
                        if (!string.IsNullOrEmpty(rfidIn))
                        {
                            var rfidInValue = JsonSerializer.Deserialize<RFIDDataModel>(rfidIn);
                            plateNumber = rfidInValue?.VehicleInfo?.PlateNumber ?? plateNumber;
                            inEpoch = rfidInValue?.Epoch;
                        }
                    }

                    // Create list vehicle is waiting
                    waitingVehicles.Add(new WaitingVehicleModel()
                    {
                        RFID = fusion.RFID,
                        PlateNumber = plateNumber,
                        LaneinDateTimeEpoch = inEpoch ?? fusion.Epoch
                    });
                }
            }

            return waitingVehicles;
        }
        private List<T>? HashGetList<T>(Func<T, bool>? action, string sortedKey, Order order = Order.Ascending)
        {
            _logger.LogInformation($"Executing {nameof(HashGetList)} method...");
            List<T>? result = new List<T>();

            try
            {
                var members = _redisDB.SortedSetRangeByScoreWithScores(sortedKey, order: order);

                foreach (var member in members)
                {
                    var hashEntries = _redisDB.HashGetAll(member.Element.ToString());

                    if (hashEntries != null && hashEntries.Any())
                    {
                        var item = RedisExtention.ConvertFromRedis<T>(hashEntries);

                        if (item != null && (action == null || action.Invoke(item)))
                        {
                            result.Add(item);
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(HashGetList)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public List<LaneInCameraDataModel> GetAllCameraModelByPattern(string pattern)
        {
            _logger.LogInformation($"Starting load all camera data...");

            try
            {
                List<LaneInCameraDataModel> result = new List<LaneInCameraDataModel>();
                var redisKeys = _redisServer.Keys(0, pattern).ToList();
                if (!redisKeys.Any())
                    return result;

                foreach (RedisKey key in redisKeys)
                {
                    string? cameraValue = _redisDB.StringGet(key).ToString();
                    if (!string.IsNullOrEmpty(cameraValue))
                    {
                        // Deserialize Camera data from Lane In
                        ANPRCameraModel? cameraData = JsonSerializer.Deserialize<ANPRCameraModel>(cameraValue);
                        if (cameraData != null && cameraData.VehicleInfo != null)
                        {
                            result.Add(new LaneInCameraDataModel()
                            {
                                CameraDeviceInfo = new DeviceModel()
                                {
                                    IpAddr = cameraData.IpAddr,
                                    MacAddr = cameraData.MacAddr
                                },
                                CameraKey = key.ToString(),
                                Epoch = cameraData.CheckpointTimeEpoch,
                                LaneId = cameraData.LaneInId,
                                VehicleInfo = cameraData.VehicleInfo,
                                IsEpayWalletRegisteredVehicle = cameraData.IsEpayWalletRegisteredVehicle
                            });
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetAllCameraModelByPattern)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion
    }
}
