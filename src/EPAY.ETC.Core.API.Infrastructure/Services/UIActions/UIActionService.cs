using EPAY.ETC.Core.API.Core.Extensions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.API.Core.Models.Configs;
using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.API.Core.Models.UI;
using EPAY.ETC.Core.API.Core.Models.Vehicle.ReconcileVehicle;
using EPAY.ETC.Core.API.Infrastructure.Common.Constants;
using EPAY.ETC.Core.API.Infrastructure.Common.Extensions;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.CustomVehicleTypes;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ETCCheckouts;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ManualBarrierControls;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Payment;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PaymentStatus;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PrintLog;
using EPAY.ETC.Core.Models.BarrierOpenStatus;
using EPAY.ETC.Core.Models.Constants;
using EPAY.ETC.Core.Models.Devices;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Fees;
using EPAY.ETC.Core.Models.Receipt.SessionReports;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.UI;
using EPAY.ETC.Core.Models.Utils;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Linq.Expressions;
using System.Text.Json;
using ValidationResult = EPAY.ETC.Core.Models.Validation.ValidationResult;

namespace EPAY.ETC.Core.API.Infrastructure.Services.UIActions
{
    public class UIActionService : IUIActionService
    {
        private readonly ILogger<UIActionService> _logger;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentStatusRepository _paymentStatusRepository;
        private readonly IAppConfigRepository _appConfigRepository;
        private readonly ICustomVehicleTypeRepository _customVehicleTypeRepository;
        private readonly IManualBarrierControlRepository _manualBarrierControlRepository;
        private readonly IDatabase _redisDB;
        private readonly IServer _redisServer;
        private readonly IOptions<UIModel> _uiOptions;
        private readonly IPrintLogRepository _appPrintLogRepository;

        public UIActionService(ILogger<UIActionService> logger,
                               IPaymentRepository paymentRepository,
                               IPaymentStatusRepository paymentStatusRepository,
                               IAppConfigRepository appConfigRepository,
                               ICustomVehicleTypeRepository customVehicleTypeRepository,
                               IManualBarrierControlRepository manualBarrierControlRepository,
                               IDatabase redisDB,
                               IServer server,
                               IOptions<UIModel> uiOptions,
                               IPrintLogRepository appPrintLogRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
            _paymentStatusRepository = paymentStatusRepository ?? throw new ArgumentNullException(nameof(paymentStatusRepository));
            _appConfigRepository = appConfigRepository ?? throw new ArgumentNullException(nameof(appConfigRepository));
            _customVehicleTypeRepository = customVehicleTypeRepository ?? throw new ArgumentNullException(nameof(customVehicleTypeRepository));
            _manualBarrierControlRepository = manualBarrierControlRepository ?? throw new ArgumentNullException(nameof(manualBarrierControlRepository));
            _redisDB = redisDB ?? throw new ArgumentNullException(nameof(redisDB));
            _redisServer = server ?? throw new ArgumentNullException(nameof(server));
            _uiOptions = uiOptions ?? throw new ArgumentNullException(nameof(uiOptions));
            _appPrintLogRepository = appPrintLogRepository ?? throw new ArgumentNullException(nameof(appPrintLogRepository));
        }

        public ValidationResult<ReconcileResultModel> ReconcileVehicleInfo(ReconcileVehicleInfoModel reconcilVehicleInfo)
        {
            _logger.LogInformation($"Executing {nameof(ReconcileVehicleInfo)} method...");

            try
            {
                var uiModelStr = (string?)_redisDB.StringGet(RedisConstant.UI_MODEL_KEY);
                UIModel? uiModel = null;
                if (!string.IsNullOrEmpty(uiModelStr))
                    uiModel = JsonSerializer.Deserialize<UIModel>(uiModelStr);

                Guid objectId = uiModel?.ObjectId ?? Guid.NewGuid();
                if (reconcilVehicleInfo.ObjectId != null && reconcilVehicleInfo.ObjectId != Guid.Empty)
                    objectId = (Guid)reconcilVehicleInfo.ObjectId;

                ReconcileResultModel result = new ReconcileResultModel();
                if (reconcilVehicleInfo?.Payment != null)
                {
                    Guid? paymentId = uiModel?.Body?.Payment?.PaymentId;

                    if (reconcilVehicleInfo.Payment.PaymentId != null && reconcilVehicleInfo.Payment.PaymentId != Guid.Empty)
                        paymentId = reconcilVehicleInfo.Payment.PaymentId;

                    result.PaymentStatus = new PaymenStatusResponseModel()
                    {
                        ObjectId = objectId,
                        PaymentStatus = new ETC.Core.Models.Fees.PaymentStatusModel()
                        {
                            PaymentId = paymentId,
                            Status = reconcilVehicleInfo.Payment?.PaymentStatus ?? PaymentStatusEnum.Unpaid,
                            PaymentMethod = reconcilVehicleInfo.Payment?.PaymentType ?? PaymentMethodEnum.Cash,
                            Amount = uiModel?.Body?.Out?.Amount,
                            Currency = uiModel?.Body?.Out?.Currency
                        }
                    };
                    return ValidationResult.Success(result);
                }
                else
                {
                    // Get FeeModel object from Redis.
                    FeeModel? feeModel = null;
                    var feeObject = _redisDB.StringGet(RedisConstant.FeeModulesKey(reconcilVehicleInfo?.ObjectId?.ToString() ?? uiModel?.ObjectId?.ToString() ?? string.Empty));

                    if (!string.IsNullOrEmpty(feeObject.ToString()))
                    {
                        feeModel = JsonSerializer.Deserialize<FeeModel>(feeObject.ToString());
                        if (feeModel != null)
                        {
                            // EmployeeId
                            feeModel.EmployeeId = !string.IsNullOrEmpty(reconcilVehicleInfo?.EmployeeId) ? reconcilVehicleInfo?.EmployeeId : feeModel.EmployeeId;
                            feeModel.IsETCFailed = false;

                            if (!int.TryParse(reconcilVehicleInfo?.Vehicle?.VehicleType, out int vehicleType))
                                vehicleType = 1;
                            feeModel.CustomVehicleType = (CustomVehicleTypeEnum)vehicleType;

                            // LandOut
                            // Update PlateNumber for FusionObject
                            _redisDB.HashSet(objectId.ToString(), nameof(FusionModel.ANPRCam1), reconcilVehicleInfo?.Vehicle?.PlateNumber);

                            string? oldRFID = feeModel.LaneOutVehicle?.RFID;

                            feeModel.LaneOutVehicle = new LaneOutVehicleModel();
                            feeModel.LaneOutVehicle.VehicleInfo = new ETC.Core.Models.VehicleInfoModel();
                            feeModel.LaneOutVehicle.VehicleInfo.PlateNumber = !string.IsNullOrEmpty(reconcilVehicleInfo?.Vehicle?.PlateNumber) ? reconcilVehicleInfo?.Vehicle?.PlateNumber : feeModel.LaneOutVehicle.VehicleInfo.PlateNumber;

                            if (!string.IsNullOrEmpty(reconcilVehicleInfo?.Vehicle?.RFID))
                                oldRFID = reconcilVehicleInfo?.Vehicle?.RFID;

                            if (!string.IsNullOrEmpty(feeModel.LaneOutVehicle.VehicleInfo.PlateNumber))
                            {
                                var rfid = _redisDB.StringGet(RedisConstant.RFIDValueKey(reconcilVehicleInfo?.Vehicle?.PlateNumber ?? string.Empty));
                                if (!string.IsNullOrEmpty(rfid))
                                {
                                    oldRFID = rfid;
                                }
                            }

                            feeModel.LaneOutVehicle.RFID = oldRFID;
                            feeModel.LaneOutVehicle.VehicleInfo.VehicleType = !string.IsNullOrEmpty(reconcilVehicleInfo?.Vehicle?.VehicleType) ? reconcilVehicleInfo?.Vehicle?.VehicleType : feeModel.LaneOutVehicle.VehicleInfo.VehicleType;
                            feeModel.LaneOutVehicle.LaneOutId = !string.IsNullOrEmpty(reconcilVehicleInfo?.Vehicle?.Out?.LaneOutId) ? reconcilVehicleInfo?.Vehicle?.Out?.LaneOutId : feeModel.LaneOutVehicle.LaneOutId;
                            feeModel.LaneOutVehicle.Epoch = DateTimeOffset.Now.ToUnixTimeSeconds();

                            // Load reconciliation image
                            var camOutStr = _redisDB.StringGet(RedisConstant.CameraOutKey(feeModel.LaneOutVehicle.VehicleInfo.PlateNumber ?? string.Empty)).ToString();

                            if (!string.IsNullOrEmpty(camOutStr))
                            {
                                var camData = JsonSerializer.Deserialize<ANPRCameraModel>(camOutStr);
                                if (camData != null)
                                {
                                    feeModel.LaneOutVehicle.Epoch = camData.CheckpointTimeEpoch;
                                    feeModel.LaneOutVehicle.VehicleInfo.VehiclePhotoUrl = camData.VehicleInfo?.VehiclePhotoUrl ?? camData.VehicleInfo?.VehicleRearPhotoUrl ?? string.Empty;
                                    feeModel.LaneOutVehicle.VehicleInfo.PlateNumberPhotoUrl = camData.VehicleInfo?.PlateNumberPhotoUrl ?? camData.VehicleInfo?.PlateNumberRearPhotoUrl ?? string.Empty;
                                    feeModel.LaneOutVehicle.VehicleInfo.PlateNumberPhotoUrl = camData.VehicleInfo?.PlateNumberPhotoUrl ?? camData.VehicleInfo?.PlateNumberRearPhotoUrl ?? string.Empty;
                                    feeModel.LaneOutVehicle.VehicleInfo.ConfidenceScore = camData.VehicleInfo?.ConfidenceScore ?? 0;
                                    feeModel.LaneOutVehicle.VehicleInfo.VehicleType = camData.VehicleInfo?.VehicleType ?? string.Empty;
                                    feeModel.LaneOutVehicle.VehicleInfo.VehicleColour = camData.VehicleInfo?.VehicleColour ?? string.Empty;
                                    feeModel.LaneOutVehicle.VehicleInfo.Weight = camData.VehicleInfo?.Weight ?? 0;
                                    feeModel.LaneOutVehicle.VehicleInfo.Model = camData.VehicleInfo?.Model ?? string.Empty;
                                    feeModel.LaneOutVehicle.VehicleInfo.Make = camData.VehicleInfo?.Make ?? string.Empty;
                                    feeModel.LaneOutVehicle.VehicleInfo.Seat = camData.VehicleInfo?.Seat ?? 0;
                                    feeModel.LaneOutVehicle.VehicleInfo.PlateColour = camData.VehicleInfo?.PlateColour ?? camData.VehicleInfo?.RearPlateColour ?? string.Empty;
                                }
                            }

                            if (!string.IsNullOrEmpty(feeModel.LaneOutVehicle.RFID))
                            {
                                var rfidOutStr = _redisDB.StringGet(RedisConstant.RFIDOutKey(feeModel.LaneOutVehicle.RFID ?? string.Empty)).ToString();
                                if (!string.IsNullOrEmpty(rfidOutStr))
                                {
                                    var rfidOut = JsonSerializer.Deserialize<RFIDDataModel>(rfidOutStr);

                                    if (rfidOut != null)
                                    {
                                        feeModel.LaneOutVehicle.Epoch = DateTimeOffset.FromUnixTimeMilliseconds(rfidOut.Epoch).ToUnixTimeSeconds();
                                        if (rfidOut.VehicleInfo != null)
                                        {
                                            if (feeModel.LaneOutVehicle.VehicleInfo == null)
                                                feeModel.LaneOutVehicle.VehicleInfo = new ETC.Core.Models.VehicleInfoModel();

                                            feeModel.LaneOutVehicle.VehicleInfo.VehicleType = rfidOut.VehicleInfo.VehicleType ?? string.Empty;
                                            feeModel.LaneOutVehicle.VehicleInfo.VehicleColour = rfidOut.VehicleInfo.VehicleColour ?? string.Empty;
                                            feeModel.LaneOutVehicle.VehicleInfo.Weight = rfidOut.VehicleInfo.Weight ?? 0;
                                            feeModel.LaneOutVehicle.VehicleInfo.Model = rfidOut.VehicleInfo.Model ?? string.Empty;
                                            feeModel.LaneOutVehicle.VehicleInfo.Make = rfidOut.VehicleInfo.Make ?? string.Empty;
                                            feeModel.LaneOutVehicle.VehicleInfo.Seat = rfidOut.VehicleInfo.Seat ?? 0;
                                            feeModel.LaneOutVehicle.VehicleInfo.PlateColour = rfidOut.VehicleInfo.PlateColour ?? rfidOut.VehicleInfo.RearPlateColour ?? string.Empty;
                                        }
                                    }
                                }
                            }

                            // LandIn
                            if ((reconcilVehicleInfo?.Vehicle?.IsWrongLaneInInfo ?? false) == false)
                            {
                                feeModel.LaneInVehicle = new LaneInVehicleModel();
                                feeModel.LaneInVehicle.VehicleInfo = new ETC.Core.Models.VehicleInfoModel();

                                feeModel.LaneInVehicle.LaneInId = !string.IsNullOrEmpty(reconcilVehicleInfo?.Vehicle?.In?.LaneInId) ? reconcilVehicleInfo?.Vehicle?.In?.LaneInId : feeModel.LaneInVehicle.LaneInId;
                                feeModel.LaneInVehicle.Epoch = (reconcilVehicleInfo?.Vehicle?.In?.LaneInDateTimeEpoch > 0) ? reconcilVehicleInfo?.Vehicle?.In?.LaneInDateTimeEpoch ?? 0 : feeModel.LaneInVehicle.Epoch;
                                feeModel.LaneInVehicle.RFID = feeModel.LaneOutVehicle.RFID;
                                feeModel.LaneInVehicle.VehicleInfo.PlateNumber = !string.IsNullOrEmpty(reconcilVehicleInfo?.Vehicle?.PlateNumber) ? reconcilVehicleInfo?.Vehicle?.PlateNumber : feeModel.LaneInVehicle.VehicleInfo.PlateNumber;
                                feeModel.LaneInVehicle.VehicleInfo.VehicleType = !string.IsNullOrEmpty(reconcilVehicleInfo?.Vehicle?.VehicleType) ? reconcilVehicleInfo?.Vehicle?.VehicleType : feeModel.LaneInVehicle.VehicleInfo.VehicleType;

                                feeModel.LaneInVehicle.Cameras = new List<LaneInCameraDataModel>();
                                LaneInCameraDataModel? firstCamInData = null;

                                if (!string.IsNullOrEmpty(reconcilVehicleInfo?.Vehicle?.In?.CamInKey))
                                {
                                    // Reset popup after choose laneIn
                                    if (uiModel != null)
                                    {
                                        _logger.LogInformation($"Reset Popup data when choose laneIn data: {reconcilVehicleInfo?.Vehicle?.In?.CamInKey}");

                                        if (uiModel.Body == null)
                                            uiModel.Body = new BodyModel();
                                        uiModel.Body.Popup = new PopupModel();

                                        _redisDB.StringSet(RedisConstant.UI_MODEL_KEY, JsonSerializer.Serialize(uiModel));
                                    }

                                    // Reload laneIn data
                                    string? camStr = _redisDB.StringGet(reconcilVehicleInfo?.Vehicle?.In?.CamInKey ?? string.Empty);
                                    if (!string.IsNullOrEmpty(camStr))
                                    {
                                        var camData = JsonSerializer.Deserialize<ANPRCameraModel>(camStr);
                                        if (camData != null)
                                        {
                                            firstCamInData = new LaneInCameraDataModel()
                                            {
                                                CameraDeviceInfo = new DeviceModel()
                                                {
                                                    IpAddr = camData.IpAddr,
                                                    MacAddr = camData.MacAddr
                                                },
                                                CameraKey = reconcilVehicleInfo?.Vehicle?.In?.CamInKey ?? string.Empty,
                                                Epoch = camData.CheckpointTimeEpoch,
                                                LaneId = camData.LaneInId,
                                                TagId = string.Empty,
                                                VehicleInfo = camData.VehicleInfo
                                            };

                                            feeModel.LaneInVehicle.Cameras.Add(firstCamInData);
                                        }
                                    }
                                }
                                else
                                    feeModel.LaneInVehicle.Cameras = GetAllCameraModelByPattern($"{RedisConstant.CameraInKey(feeModel.LaneOutVehicle.VehicleInfo.PlateNumber ?? string.Empty)}*").OrderByDescending(x => x.Epoch).ToList();

                                if (firstCamInData == null)
                                    firstCamInData = feeModel.LaneInVehicle.Cameras?.FirstOrDefault();

                                bool isEmptyLaneIn = false;
                                if (firstCamInData != null)
                                {
                                    isEmptyLaneIn = false;
                                    feeModel.LaneInVehicle.Epoch = firstCamInData.Epoch;
                                    feeModel.LaneInVehicle.VehicleInfo.VehiclePhotoUrl = firstCamInData.VehicleInfo?.VehiclePhotoUrl ?? firstCamInData.VehicleInfo?.VehicleRearPhotoUrl ?? string.Empty;
                                    feeModel.LaneInVehicle.VehicleInfo.PlateNumberPhotoUrl = firstCamInData.VehicleInfo?.PlateNumberPhotoUrl ?? firstCamInData.VehicleInfo?.PlateNumberRearPhotoUrl ?? string.Empty;
                                    feeModel.LaneInVehicle.VehicleInfo.ConfidenceScore = firstCamInData.VehicleInfo?.ConfidenceScore ?? 0;
                                    feeModel.LaneInVehicle.VehicleInfo.VehicleType = firstCamInData.VehicleInfo?.VehicleType ?? string.Empty;
                                    feeModel.LaneInVehicle.VehicleInfo.VehicleColour = firstCamInData.VehicleInfo?.VehicleColour ?? string.Empty;
                                    feeModel.LaneInVehicle.VehicleInfo.Weight = firstCamInData.VehicleInfo?.Weight ?? 0;
                                    feeModel.LaneInVehicle.VehicleInfo.Model = firstCamInData.VehicleInfo?.Model ?? string.Empty;
                                    feeModel.LaneInVehicle.VehicleInfo.Make = firstCamInData.VehicleInfo?.Make ?? string.Empty;
                                    feeModel.LaneInVehicle.VehicleInfo.Seat = firstCamInData.VehicleInfo?.Seat ?? 0;
                                    feeModel.LaneInVehicle.VehicleInfo.PlateColour = firstCamInData.VehicleInfo?.PlateColour ?? firstCamInData.VehicleInfo?.RearPlateColour ?? string.Empty;
                                }
                                else
                                    isEmptyLaneIn = true;

                                if (!string.IsNullOrEmpty(feeModel.LaneOutVehicle.RFID))
                                {
                                    var rfidOutStr = _redisDB.StringGet(RedisConstant.RFIDInKey(feeModel.LaneOutVehicle.RFID)).ToString();
                                    if (!string.IsNullOrEmpty(rfidOutStr))
                                    {
                                        var rfidIn = JsonSerializer.Deserialize<RFIDDataModel>(rfidOutStr);

                                        if (rfidIn != null)
                                        {
                                            isEmptyLaneIn = false;
                                            feeModel.LaneInVehicle.Epoch = DateTimeOffset.FromUnixTimeMilliseconds(rfidIn.Epoch).ToUnixTimeSeconds();
                                            if (rfidIn.VehicleInfo != null)
                                            {
                                                feeModel.LaneOutVehicle.VehicleInfo.VehicleType = rfidIn.VehicleInfo.VehicleType ?? string.Empty;
                                                feeModel.LaneOutVehicle.VehicleInfo.VehicleColour = rfidIn.VehicleInfo.VehicleColour ?? string.Empty;
                                                feeModel.LaneOutVehicle.VehicleInfo.Weight = rfidIn.VehicleInfo.Weight ?? 0;
                                                feeModel.LaneOutVehicle.VehicleInfo.Model = rfidIn.VehicleInfo.Model ?? string.Empty;
                                                feeModel.LaneOutVehicle.VehicleInfo.Make = rfidIn.VehicleInfo.Make ?? string.Empty;
                                                feeModel.LaneOutVehicle.VehicleInfo.Seat = rfidIn.VehicleInfo.Seat ?? 0;
                                                feeModel.LaneOutVehicle.VehicleInfo.PlateColour = rfidIn.VehicleInfo.PlateColour ?? rfidIn.VehicleInfo.RearPlateColour ?? string.Empty;
                                            }
                                        }
                                    }
                                }

                                if (isEmptyLaneIn)
                                    feeModel.LaneInVehicle = null;
                            }
                            else
                                feeModel.LaneInVehicle = null;

                            result.Fee = feeModel;

                            return ValidationResult.Success(result);
                        }
                        else
                        {
                            _logger.LogError($"Fee object from Redis is null {nameof(ReconcileVehicleInfo)} method.");
                            return ValidationResult.Success(result);
                        }
                    }
                    else
                    {
                        _logger.LogError($"Fee object from Redis is null {nameof(ReconcileVehicleInfo)} method.");
                        return ValidationResult.Success(result);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(ReconcileVehicleInfo)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ValidationResult<ManipulateBarrierResponseModel>> ManipulateBarrierAsync(BarrierRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(ManipulateBarrierAsync)} method...");

                ManipulateBarrierResponseModel result = new ManipulateBarrierResponseModel();
                string laneId = Environment.GetEnvironmentVariable(CoreConstant.ENVIRONMENT_LANE_OUT) ?? "1";

                result.BarrierOpenStatus = new BarrierOpenStatus()
                {
                    Status = request.Action!,
                    Limit = request.Limit ?? 1
                };

                PaymentMethodEnum? paymentMethod = null;
                switch (request.ManualBarrierType)
                {
                    case ManualBarrierTypeEnum.Priority:
                        paymentMethod = PaymentMethodEnum.Priority;
                        break;
                    case ManualBarrierTypeEnum.FreeEntry:
                        paymentMethod = PaymentMethodEnum.FreeEntry;
                        break;
                }

                await _manualBarrierControlRepository.AddAsync(new Core.Models.ManualBarrierControl.ManualBarrierControlModel()
                {
                    Action = request.Action,
                    CreatedDate = DateTime.Now,
                    EmployeeId = request.EmployeeId,
                    LaneOutId = request.LaneId ?? laneId,
                    Id = Guid.NewGuid(),
                    ManualBarrierType = request.ManualBarrierType.ToString()
                });

                var processingObjectId = _redisDB.StringGet(RedisConstant.FUSION_PROCESSING).ToString();
                Guid objectId;
                ValidationResult<UIModel>? uiModel;
                UIModel? uiModelData;

                switch (request.Action)
                {
                    case BarrierActionEnum.Open:
                        _logger.LogInformation($"Save to Redis with Key={RedisConstant.HASH_BARRIER_OPEN_STATUS}, Value={JsonSerializer.Serialize(result.BarrierOpenStatus)}");

                        _redisDB.HashSet(RedisConstant.HASH_BARRIER_OPEN_STATUS, result.BarrierOpenStatus.ToHashEntries());

                        if (paymentMethod != null)
                        {
                            _logger.LogInformation($"Save to Redis with Key={RedisConstant.MANUALBARRIER_PAYMENT_METHOD}, Value={paymentMethod}");

                            _redisDB.StringSet(RedisConstant.MANUALBARRIER_PAYMENT_METHOD, paymentMethod.ToString());
                        }

                        if (request.ManualBarrierType != ManualBarrierTypeEnum.OneTimePass)
                        {
                            _logger.LogInformation($"Processing logic for {request.ManualBarrierType}({request.ManualBarrierType.ToDescriptionString()})");

                            if (!string.IsNullOrEmpty(processingObjectId) && Guid.TryParse(processingObjectId, out objectId))
                            {
                                uiModel = await LoadCurrentUIAsync();
                                uiModelData = uiModel?.Data;
                                result.Payment = new PaymentModel()
                                {
                                    ObjectId = objectId,
                                    Amount = 0,
                                    PaymentMethod = paymentMethod,
                                    CheckOutTime = uiModelData?.Body?.Out?.LaneOutDateEpoch ?? DateTimeOffset.Now.ToUnixTimeSeconds(),
                                    PaymentId = uiModelData?.Body?.Payment?.PaymentId ?? Guid.Empty
                                };

                                _logger.LogInformation($"Send message to Payment core with message={JsonSerializer.Serialize(result.Payment)}");

                                var lastLoopStatus = _redisDB.StringGet(RedisConstant.LAST_LOOP_UNPAID);
                                if (bool.TryParse(lastLoopStatus, out bool lastLoopStatusValue) && lastLoopStatusValue)
                                {
                                    var feeModelStr = _redisDB.StringGet(RedisConstant.FeeModulesKey(processingObjectId)).ToString();
                                    if (!string.IsNullOrEmpty(feeModelStr))
                                    {
                                        result.Fee = JsonSerializer.Deserialize<FeeModel>(feeModelStr);
                                        if (result.Fee != null)
                                        {
                                            result.Fee.FeeType = FeeTypeEnum.FeeCommitment;
                                        }
                                        _logger.LogInformation($"Send message to Fees core with message={JsonSerializer.Serialize(result.Fee)}");
                                    }
                                }
                            }

                            uiModel = await LoadCurrentUIAsync();
                            uiModelData = uiModel?.Data;
                            if (uiModelData != null)
                            {
                                if (uiModelData.Body == null)
                                    uiModelData.Body = new EPAY.ETC.Core.Models.UI.BodyModel();

                                uiModelData.Body.InformationBoard = new InformationBoard()
                                {
                                    Message = new MessageModel()
                                    {
                                        Alert = AlertEnum.Success,
                                        Heading = "Thông báo",
                                        SubHeading1 = "Đang mở cho đoàn xe ưu tiên"
                                    }
                                };

                                switch (paymentMethod)
                                {
                                    case PaymentMethodEnum.Priority:
                                        uiModelData.Body.InformationBoard.Message.SubHeading1 = "Đang mở cho đoàn xe ưu tiên";
                                        break;
                                    case PaymentMethodEnum.FreeEntry:
                                        uiModelData.Body.InformationBoard.Message.SubHeading1 = "Đang xả trạm";
                                        break;
                                }

                                result.UI = uiModelData;
                                _redisDB.StringSet(RedisConstant.UI_MODEL_KEY, JsonSerializer.Serialize(uiModelData));
                            }
                        }
                        break;

                    case BarrierActionEnum.Close:
                        _logger.LogInformation($"Remove key from Redis: Key={RedisConstant.HASH_BARRIER_OPEN_STATUS}, {RedisConstant.MANUALBARRIER_PAYMENT_METHOD}");
                        _redisDB.KeyDelete(RedisConstant.HASH_BARRIER_OPEN_STATUS);
                        _redisDB.KeyDelete(RedisConstant.MANUALBARRIER_PAYMENT_METHOD);

                        _logger.LogInformation($"Processing re-send fee calculate if exists trans in queue...");
                        if (!string.IsNullOrEmpty(processingObjectId) && Guid.TryParse(processingObjectId, out objectId))
                        {
                            var feeModelStr = _redisDB.StringGet(RedisConstant.FeeModulesKey(processingObjectId)).ToString();
                            if (!string.IsNullOrEmpty(feeModelStr))
                            {
                                result.Fee = JsonSerializer.Deserialize<FeeModel>(feeModelStr);
                                if (result.Fee != null)
                                {
                                    result.Fee.FeeType = FeeTypeEnum.FeeCalculation;
                                }
                                _logger.LogInformation($"Send message to Fees core with message={JsonSerializer.Serialize(result.Fee)}");
                            }
                        }

                        uiModel = await LoadCurrentUIAsync();
                        uiModelData = uiModel?.Data;
                        if (uiModelData != null)
                        {
                            if (uiModelData.Body == null)
                                uiModelData.Body = new EPAY.ETC.Core.Models.UI.BodyModel();

                            uiModelData.Body.InformationBoard = new InformationBoard();

                            result.UI = uiModelData;
                            _redisDB.StringSet(RedisConstant.UI_MODEL_KEY, JsonSerializer.Serialize(uiModelData));
                        }
                        break;
                }

                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(ManipulateBarrierAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ValidationResult<LaneSessionReportModel>> PrintLaneSessionReportAsync(LaneSessionReportRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(PrintLaneSessionReportAsync)} method...");

                Expression<Func<AppConfigModel, bool>> expression = s => s.IsApply == true;

                var currentUI = await LoadCurrentUIAsync();
                var appConfig = (await _appConfigRepository.GetAllAsync(expression)).FirstOrDefault();
                var vehicleTypes = await _customVehicleTypeRepository.GetAllAsync();
                vehicleTypes = vehicleTypes.ToList().Where(x => !string.IsNullOrEmpty(x.Name.GetDescription())).OrderBy(x => x.Name);

                var paymentStatuses = await _paymentStatusRepository.GetAllWithNavigationAsync(request);

                DateTime fromDate = request.FromDateTimeEpoch.ToSpecificDateTime();
                DateTime toDate = request.ToDateTimeEpoch.ToSpecificDateTime();

                LaneSessionReportModel result = new LaneSessionReportModel()
                {
                    PrintType = ReceiptTypeEnum.SessionReport,
                    Layout = new LaneSessionLayoutModel()
                    {
                        Header = new ETC.Core.Models.Receipt.HeaderModel
                        {
                            Heading = appConfig?.HeaderHeading?.ToUpper(),
                            SubHeading = appConfig?.HeaderSubHeading?.ToUpper(),
                            Line1 = appConfig?.HeaderLine1,
                            Line2 = appConfig?.HeaderLine2
                        },
                        Footer = new ETC.Core.Models.Receipt.FooterModel()
                        {
                            Line1 = $"{appConfig?.FooterLine1?.Trim()} Ngày {toDate.Day} tháng {toDate.Month} năm {toDate.Year}",
                            Line2 = $"{appConfig?.FooterLine2?.Trim()}"
                        },
                        Body = new LaneSessionBodyModel()
                    }
                };

                string employeeName = currentUI.Data?.Header?.EmployeeName ?? $"{currentUI.Data?.Authentication?.FirstName} {currentUI.Data?.Authentication?.LastName}";

                result.Layout.Footer.Line3 = employeeName;
                result.Layout.Body.Heading = ReceiptTypeEnum.SessionReport.ToEnumMemberAttrValue().ToUpper();

                // TODO: Need to get name of Shift
                result.Layout.Body.SubHeading1 = $"Ngày: {fromDate.ToString("dd/MM/yyyy")}  Ca: {currentUI.Data?.Header?.ShiftName ?? "01"}  Trạm: {request.LaneOutId}";
                result.Layout.Body.SubHeading2 = $"Từ giờ: {fromDate.ToString("HH:mm:ss")}  Đến giờ: {toDate.ToString("HH:mm:ss")}";

                result.Layout.Body.SubHeading3 = employeeName;

                result.Layout.Body.Columns = new List<string>() { "Số", "Loại xe", "Số lượng", "T.tiền" };

                var grandTotal = paymentStatuses.Sum(x => x.Amount);
                result.Layout.Body.Data = new LaneSessionDataModel()
                {
                    Qty = paymentStatuses.Count(),
                    GrandTotal = grandTotal,
                    BottomLine = grandTotal.ToMoneyString()
                };

                var paymentMethodCashs = paymentStatuses.Where(x => x.PaymentMethod == PaymentMethodEnum.Cash);
                var paymentMethodNoneCashs = paymentStatuses.Where(x => x.PaymentMethod != PaymentMethodEnum.Cash);

                result.Layout.Body.Data.Payments = new List<LaneSessionPaymentDataModel>()
                {
                    new LaneSessionPaymentDataModel()
                    {
                        Name = $"Hình thức thu phí - Tiền mặt",
                        Qty = paymentMethodCashs.Count(),
                        Total = paymentMethodCashs.Sum(p => p.Amount),
                        Details = vehicleTypes.Select(s =>
                        {
                            var subPayments = paymentMethodCashs.Where(x => x.Payment?.CustomVehicleTypeId == s.Id);

                            return new LaneSessionPaymentDetailDataModel()
                            {
                                Type = s.Name.GetDescription(),
                                Qty = subPayments.Count(),
                                Subtotal = subPayments.Sum(p => p.Amount)
                            };
                        }).ToList()
                    },
                    new LaneSessionPaymentDataModel()
                    {
                        Name = $"Hình thức thu phí - Phi tiền mặt",
                        Qty = paymentMethodNoneCashs.Count(),
                        Total = paymentMethodNoneCashs.Sum(p => p.Amount),
                        Details = vehicleTypes.Select(s =>
                        {
                            var subPayments = paymentMethodNoneCashs.Where(x => x.Payment?.CustomVehicleTypeId == s.Id);

                            return new LaneSessionPaymentDetailDataModel()
                            {
                                Type = s.Name.GetDescription(),
                                Qty = subPayments.Count(),
                                Subtotal = subPayments.Sum(p => p.Amount)
                            };
                        }).ToList()
                    }
                };

                // Add Report to DB

                await _appPrintLogRepository.AddAsync(new Core.Models.PrintLog.PrintLogModel()
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    PrintType = PrintLogEnum.Report,
                    DataJson = JsonSerializer.Serialize(result),
                    LaneOutId = request.LaneOutId,
                    EmployeeId = request.EmployeeId
                });

                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(PrintLaneSessionReportAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public ValidationResult<PaymenStatusResponseModel> UpdatePaymentMethod(PaymentStatusUIRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(UpdatePaymentMethod)} method...");

                PaymenStatusResponseModel result = new PaymenStatusResponseModel()
                {
                    ObjectId = request.ObjectId ?? Guid.Empty,
                    PaymentStatus = new ETC.Core.Models.Fees.PaymentStatusModel()
                    {
                        PaymentId = request.PaymentId,
                        Amount = request.Amount,
                        PaymentMethod = request.PaymentMethod,
                        Status = request.Status,
                        CreatedDate = DateTime.Now,
                        PaymentDate = DateTime.Now
                    }
                };

                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(UpdatePaymentMethod)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ValidationResult<UIModel>> LoadCurrentUIAsync(AuthenticatedEmployeeResponseModel? authenticatedEmployee = null)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(LoadCurrentUIAsync)} method...");

                UIModel? result = null;
                string laneId = Environment.GetEnvironmentVariable(CoreConstant.ENVIRONMENT_LANE_OUT) ?? "1";

                var paidHistories = await _paymentRepository.GetPaidVehicleHistoryAsync(laneId);

                _redisDB.StringSet(RedisConstant.PAID_VEHICLE_HISTORY_KEY, JsonSerializer.Serialize(paidHistories));

                var waitingVehicles = GetWaitingVehicles();

                var uiModelStr = _redisDB.StringGet(RedisConstant.UI_MODEL_KEY);

                if (!string.IsNullOrEmpty(uiModelStr.ToString()))
                {
                    result = JsonSerializer.Deserialize<UIModel>(uiModelStr.ToString());
                }

                if (result == null)
                {
                    uiModelStr = _redisDB.StringGet(RedisConstant.UI_MODEL_TEMPLATE_KEY);

                    if (!string.IsNullOrEmpty(uiModelStr.ToString()))
                    {
                        result = JsonSerializer.Deserialize<UIModel>(uiModelStr.ToString());
                    }
                }

                if (result == null)
                    result = _uiOptions.Value;

                if (authenticatedEmployee != null)
                {
                    result.Authentication = authenticatedEmployee;

                    if (result.Command == null)
                        result.Command = new ETC.Core.Models.UI.Command.CommandModel();
                    if (result.Command.Logon == null)
                        result.Command.Logon = new ETC.Core.Models.UI.Command.LogonModel();

                    result.Command.Logon.Action = LogonStatusEnum.Login;
                    result.Command.LandId = laneId;

                    if (result.Header == null)
                        result.Header = new EPAY.ETC.Core.Models.UI.HeaderModel();
                    result.Header.EmployeeName = $"{authenticatedEmployee.FirstName} {authenticatedEmployee.LastName}";
                    result.Header.ShiftName = $"Ca 1";
                }

                if (result.Body == null)
                    result.Body = new EPAY.ETC.Core.Models.UI.BodyModel();

                result.Body.PaidVehicleHistory = paidHistories;
                result.Body.WaitingVehicles = waitingVehicles;

                _redisDB.StringSet(RedisConstant.UI_MODEL_KEY, JsonSerializer.Serialize(result));

                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(LoadCurrentUIAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public void AddOrUpdateCurrentUI(UIModel input)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(AddOrUpdateCurrentUI)} method...");
                _redisDB.StringSet(RedisConstant.UI_MODEL_KEY, JsonSerializer.Serialize(input));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(AddOrUpdateCurrentUI)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public string GetFeeProcessing()
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetFeeProcessing)} method...");
                var result = _redisDB.StringGet(AppConstant.REDIS_KEY_FUSION_PROCESSING);

                return result.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetFeeProcessing)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        #region Private method
        public List<WaitingVehicleModel> GetWaitingVehicles()
        {
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
