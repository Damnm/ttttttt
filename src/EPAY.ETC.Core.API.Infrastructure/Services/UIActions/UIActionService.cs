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
        private readonly IOptions<UIModel> _uiOptions;

        public UIActionService(ILogger<UIActionService> logger,
                               IPaymentRepository paymentRepository,
                               IPaymentStatusRepository paymentStatusRepository,
                               IAppConfigRepository appConfigRepository,
                               ICustomVehicleTypeRepository customVehicleTypeRepository,
                               IManualBarrierControlRepository manualBarrierControlRepository,
                               IDatabase redisDB,
                               IOptions<UIModel> uiOptions)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
            _paymentStatusRepository = paymentStatusRepository ?? throw new ArgumentNullException(nameof(paymentStatusRepository));
            _appConfigRepository = appConfigRepository ?? throw new ArgumentNullException(nameof(appConfigRepository));
            _customVehicleTypeRepository = customVehicleTypeRepository ?? throw new ArgumentNullException(nameof(customVehicleTypeRepository));
            _manualBarrierControlRepository = manualBarrierControlRepository ?? throw new ArgumentNullException(nameof(manualBarrierControlRepository));
            _redisDB = redisDB ?? throw new ArgumentNullException(nameof(redisDB));
            _uiOptions = uiOptions ?? throw new ArgumentNullException(nameof(uiOptions));
        }

        public async Task<ValidationResult<ReconcileResultModel>> ReconcileVehicleInfoAsync(ReconcileVehicleInfoModel reconcilVehicleInfo)
        {
            _logger.LogInformation($"Executing {nameof(ReconcileVehicleInfoAsync)} method...");

            try
            {
                var uiModelStr = (string?)await _redisDB.StringGetAsync(RedisConstant.UI_MODEL_KEY);
                UIModel? uiModel = null;
                if (!string.IsNullOrEmpty(uiModelStr))
                    uiModel = JsonSerializer.Deserialize<UIModel>(uiModelStr);

                ReconcileResultModel result = new ReconcileResultModel();
                if (reconcilVehicleInfo?.Payment != null)
                {
                    result.PaymentStatus = new PaymenStatusResponseModel()
                    {
                        ObjectId = reconcilVehicleInfo.ObjectId ?? uiModel?.ObjectId ?? Guid.NewGuid(),
                        PaymentStatus = new ETC.Core.Models.Fees.PaymentStatusModel()
                        {
                            PaymentId = reconcilVehicleInfo.Payment.PaymentId ?? uiModel?.Body?.Payment?.PaymentId,
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
                    var feeObject = await _redisDB.StringGetAsync(RedisConstant.StringType_FeeModules(reconcilVehicleInfo?.ObjectId.ToString() ?? uiModel?.ObjectId.ToString() ?? string.Empty));

                    if (!string.IsNullOrEmpty(feeObject.ToString()))
                    {
                        feeModel = JsonSerializer.Deserialize<FeeModel>(feeObject.ToString());
                        if (feeModel != null)
                        {
                            // EmployeeId
                            feeModel.EmployeeId = !string.IsNullOrEmpty(reconcilVehicleInfo?.EmployeeId) ? reconcilVehicleInfo?.EmployeeId : feeModel.EmployeeId;

                            if (!int.TryParse(reconcilVehicleInfo?.Vehicle?.VehicleType, out int vehicleType))
                                vehicleType = 1;
                            feeModel.CustomVehicleType = (CustomVehicleTypeEnum)vehicleType;

                            // LandOut
                            if (feeModel.LaneOutVehicle == null)
                                feeModel.LaneOutVehicle = new LaneOutVehicleModel();
                            if (feeModel.LaneOutVehicle.VehicleInfo == null)
                                feeModel.LaneOutVehicle.VehicleInfo = new ETC.Core.Models.VehicleInfoModel();

                            // Update PlateNumber for FusionObject
                            await _redisDB.HashSetAsync(reconcilVehicleInfo?.ObjectId.ToString() ?? uiModel?.ObjectId.ToString() ?? string.Empty, nameof(FusionModel.ANPRCam1), reconcilVehicleInfo?.Vehicle?.PlateNumber);

                            feeModel.LaneOutVehicle.VehicleInfo.PlateNumber = !string.IsNullOrEmpty(reconcilVehicleInfo?.Vehicle?.PlateNumber) ? reconcilVehicleInfo?.Vehicle?.PlateNumber : feeModel.LaneOutVehicle.VehicleInfo.PlateNumber;
                            feeModel.LaneOutVehicle.RFID = !string.IsNullOrEmpty(reconcilVehicleInfo?.Vehicle?.RFID) ? reconcilVehicleInfo?.Vehicle?.PlateNumber : feeModel.LaneOutVehicle.RFID;
                            feeModel.LaneOutVehicle.VehicleInfo.VehicleType = !string.IsNullOrEmpty(reconcilVehicleInfo?.Vehicle?.VehicleType) ? reconcilVehicleInfo?.Vehicle?.VehicleType : feeModel.LaneOutVehicle.VehicleInfo.VehicleType;
                            feeModel.LaneOutVehicle.LaneOutId = !string.IsNullOrEmpty(reconcilVehicleInfo?.Vehicle?.Out?.LaneOutId) ? reconcilVehicleInfo?.Vehicle?.Out?.LaneOutId : feeModel.LaneOutVehicle.LaneOutId;
                            feeModel.LaneOutVehicle.Epoch = (reconcilVehicleInfo?.Vehicle?.Out?.LaneOutDateTimeEpoch > 0) ? reconcilVehicleInfo.Vehicle.Out.LaneOutDateTimeEpoch ?? 0 : feeModel.LaneOutVehicle.Epoch;

                            // Load reconciliation image
                            var camOutStr = _redisDB.StringGet(RedisConstant.StringType_CameraOutKey(feeModel.LaneOutVehicle.VehicleInfo.PlateNumber ?? string.Empty)).ToString();

                            if (!string.IsNullOrEmpty(camOutStr))
                            {
                                var camData = JsonSerializer.Deserialize<ANPRCameraModel>(camOutStr);
                                if (camData != null)
                                {
                                    feeModel.LaneOutVehicle.VehicleInfo.VehiclePhotoUrl = camData.VehicleInfo?.VehiclePhotoUrl ?? camData.VehicleInfo?.VehicleRearPhotoUrl ?? string.Empty;
                                    feeModel.LaneOutVehicle.VehicleInfo.PlateNumberPhotoUrl = camData.VehicleInfo?.PlateNumberPhotoUrl ?? camData.VehicleInfo?.PlateNumberRearPhotoUrl ?? string.Empty;
                                }
                            }

                            // LandIn
                            if (reconcilVehicleInfo?.Vehicle?.In != null)
                            {
                                if (feeModel.LaneInVehicle == null)
                                    feeModel.LaneInVehicle = new LaneInVehicleModel();
                                if (feeModel.LaneInVehicle.VehicleInfo == null)
                                    feeModel.LaneInVehicle.VehicleInfo = new ETC.Core.Models.VehicleInfoModel();

                                feeModel.LaneInVehicle.LaneInId = !string.IsNullOrEmpty(reconcilVehicleInfo?.Vehicle?.In?.LaneInId) ? reconcilVehicleInfo?.Vehicle?.In?.LaneInId : feeModel.LaneInVehicle.LaneInId;
                                feeModel.LaneInVehicle.Epoch = (reconcilVehicleInfo?.Vehicle?.In?.LaneInDateTimeEpoch > 0) ? reconcilVehicleInfo?.Vehicle?.In?.LaneInDateTimeEpoch ?? 0 : feeModel.LaneInVehicle.Epoch;
                                feeModel.LaneInVehicle.RFID = !string.IsNullOrEmpty(reconcilVehicleInfo?.Vehicle?.RFID) ? reconcilVehicleInfo?.Vehicle?.PlateNumber : feeModel.LaneInVehicle.RFID;
                                feeModel.LaneInVehicle.VehicleInfo.PlateNumber = !string.IsNullOrEmpty(reconcilVehicleInfo?.Vehicle?.PlateNumber) ? reconcilVehicleInfo?.Vehicle?.PlateNumber : feeModel.LaneInVehicle.VehicleInfo.PlateNumber;
                                feeModel.LaneInVehicle.VehicleInfo.VehicleType = !string.IsNullOrEmpty(reconcilVehicleInfo?.Vehicle?.VehicleType) ? reconcilVehicleInfo?.Vehicle?.VehicleType : feeModel.LaneInVehicle.VehicleInfo.VehicleType;

                                var camInStr = _redisDB.StringGet(RedisConstant.StringType_CameraInKey(feeModel.LaneOutVehicle.VehicleInfo.PlateNumber ?? string.Empty)).ToString();

                                if (!string.IsNullOrEmpty(camInStr))
                                {
                                    var camData = JsonSerializer.Deserialize<ANPRCameraModel>(camInStr);
                                    if (camData != null)
                                    {
                                        feeModel.LaneInVehicle.VehicleInfo.VehiclePhotoUrl = camData.VehicleInfo?.VehiclePhotoUrl ?? camData.VehicleInfo?.VehicleRearPhotoUrl ?? string.Empty;
                                        feeModel.LaneInVehicle.VehicleInfo.PlateNumberPhotoUrl = camData.VehicleInfo?.PlateNumberPhotoUrl ?? camData.VehicleInfo?.PlateNumberRearPhotoUrl ?? string.Empty;
                                    }
                                }
                            }
                            else
                                feeModel.LaneInVehicle = null;

                            result.Fee = feeModel;

                            return ValidationResult.Success(result);
                        }
                        else
                        {
                            _logger.LogError($"Fee object from Redis is null {nameof(ReconcileVehicleInfoAsync)} method.");
                            return ValidationResult.Success(result);
                        }
                    }
                    else
                    {
                        _logger.LogError($"Fee object from Redis is null {nameof(ReconcileVehicleInfoAsync)} method.");
                        return ValidationResult.Success(result);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(ReconcileVehicleInfoAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ValidationResult<ManipulateBarrierResponseModel>> ManipulateBarrier(BarrierRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(ManipulateBarrier)} method...");

                ManipulateBarrierResponseModel result = new ManipulateBarrierResponseModel();

                result.BarrierOpenStatus = new BarrierOpenStatus()
                {
                    Status = request.Action!,
                    Limit = request.Limit ?? 1
                };

                await _redisDB.HashSetAsync(CoreConstant.HASH_BARRIER_OPEN_STATUS, result.ToHashEntries());
                await _manualBarrierControlRepository.AddAsync(new Core.Models.ManualBarrierControl.ManualBarrierControlModel()
                {
                    Action = request.Action,
                    CreatedDate = DateTime.Now,
                    EmployeeId = request.EmployeeId,
                    LaneOutId = request.LaneId ?? string.Empty,
                    Id = Guid.NewGuid()
                });

                if (result.BarrierOpenStatus.Status == BarrierActionEnum.Open && request.ManualBarrierType != ManualBarrierTypeEnum.OneTimePass)
                {
                    var processingObjectId = _redisDB.StringGet(RedisConstant.FUSION_PROCESSING).ToString();
                    if (!string.IsNullOrEmpty(processingObjectId) && Guid.TryParse(processingObjectId, out Guid objectId))
                    {
                        var uiModel = await LoadCurrentUIAsync();

                        result.Payment = new PaymentModel()
                        {
                            ObjectId = objectId,
                            Amount = 0,
                            PaymentMethod = PaymentMethodEnum.Priority,
                            CheckOutTime = uiModel?.Data?.Body?.Out?.LaneOutDateEpoch ?? DateTimeOffset.Now.ToUnixTimeSeconds(),
                            PaymentId = uiModel?.Data?.Body?.Payment?.PaymentId ?? Guid.Empty
                        };

                        switch (request.ManualBarrierType)
                        {
                            case ManualBarrierTypeEnum.FreeEntry:
                                result.Payment.PaymentMethod = PaymentMethodEnum.FreeEntry;
                                break;
                        }

                        var lastLoopStatus = _redisDB.StringGet(RedisConstant.LAST_LOOP_UNPAID);
                        if (bool.TryParse(lastLoopStatus, out bool lastLoopStatusValue) && lastLoopStatusValue)
                        {
                            var feeModelStr = _redisDB.StringGet(RedisConstant.StringType_FeeModules(processingObjectId)).ToString();
                            if (!string.IsNullOrEmpty(feeModelStr))
                            {
                                result.Fee = JsonSerializer.Deserialize<FeeModel>(feeModelStr);
                                if (result.Fee != null)
                                {
                                    result.Fee.FeeType = FeeTypeEnum.FeeCommitment;
                                }
                            }
                        }
                    }
                }

                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(ManipulateBarrier)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ValidationResult<LaneSessionReportModel>> PrintLaneSessionReport(LaneSessionReportRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(PrintLaneSessionReport)} method...");

                Expression<Func<AppConfigModel, bool>> expression = s => s.IsApply == true;

                var appConfig = (await _appConfigRepository.GetAllAsync(expression)).FirstOrDefault();
                var vehicleTypes = await _customVehicleTypeRepository.GetAllAsync();
                vehicleTypes = vehicleTypes.ToList().Where(x => !string.IsNullOrEmpty(x.Name.GetDescription())).OrderBy(x => x.Name);

                var paymentStatuses = await _paymentStatusRepository.GetAllWithNavigationAsync(request);

                DateTime fromDate = DateTimeOffset.FromUnixTimeSeconds(request.FromDateTimeEpoch).DateTime;
                DateTime toDate = DateTimeOffset.FromUnixTimeSeconds(request.ToDateTimeEpoch).DateTime;

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

                var firstPaymentStatus = paymentStatuses.Where(x => x.Payment != null && x.Payment.Fee != null).FirstOrDefault();

                // TODO: Need to get fullname of employee
                result.Layout.Footer.Line3 = "Nguyen Van A";

                result.Layout.Body.Heading = ReceiptTypeEnum.SessionReport.ToEnumMemberAttrValue().ToUpper();

                // TODO: Need to get name of Shift
                result.Layout.Body.SubHeading1 = $"Ngày: {fromDate.ToString("dd/MM/yyyy")}  Ca: 01  Trạm: {firstPaymentStatus?.Payment.LaneOutId}";
                result.Layout.Body.SubHeading2 = $"Từ giờ: {fromDate.ToString("HH:mm:ss")}  Đến giờ: {toDate.ToString("HH:mm:ss")}";

                // TODO: Need to get fullname of employee
                result.Layout.Body.SubHeading3 = $"Nguyen Van A";

                result.Layout.Body.Columns = new List<string>() { "Số", "Loại xe", "Số lượng", "T.tiền" };

                var grandTotal = paymentStatuses.Sum(x => x.Amount);
                result.Layout.Body.Data = new LaneSessionDataModel()
                {
                    Qty = paymentStatuses.Count(),
                    GrandTotal = grandTotal,
                    BottomLine = grandTotal.ToMoneyString()
                };

                var paymentMethodCashs = paymentStatuses
                    .Where(x => x.PaymentMethod == PaymentMethodEnum.Cash);

                var paymentMethodNoneCashs = paymentStatuses
                    .Where(x => x.PaymentMethod != PaymentMethodEnum.Cash);

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

                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(PrintLaneSessionReport)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public Task<ValidationResult<PaymenStatusResponseModel>> UpdatePaymentMethod(PaymentStatusUIRequestModel request)
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

                return Task.FromResult(ValidationResult.Success(result));
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
                string laneId = Environment.GetEnvironmentVariable("LANEOUTID_ENVIRONMENT") ?? "1";

                var paidHistories = await _paymentRepository.GetPaidVehicleHistoryAsync(laneId);

                _redisDB.StringSet(RedisConstant.PAID_VEHICLE_HISTORY_KEY, JsonSerializer.Serialize(paidHistories));

                var waitingVehicles = await GetWaitingVehicles();

                var uiModelStr = await _redisDB.StringGetAsync(RedisConstant.UI_MODEL_KEY);

                if (!string.IsNullOrEmpty(uiModelStr.ToString()))
                {
                    result = JsonSerializer.Deserialize<UIModel>(uiModelStr.ToString());
                }

                if (result == null)
                {
                    uiModelStr = await _redisDB.StringGetAsync(RedisConstant.UI_MODEL_TEMPLATE_KEY);

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
                        result.Header = new HeaderModel();
                    result.Header.EmployeeName = $"{authenticatedEmployee.FirstName} {authenticatedEmployee.LastName}";
                }

                if (result.Body == null)
                    result.Body = new BodyModel();

                result.Body.PaidVehicleHistory = paidHistories;
                result.Body.WaitingVehicles = waitingVehicles;

                await _redisDB.StringSetAsync(RedisConstant.UI_MODEL_KEY, JsonSerializer.Serialize(result));

                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(LoadCurrentUIAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task AddOrUpdateCurrentUIAsync(UIModel input)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(AddOrUpdateCurrentUIAsync)} method...");
                await _redisDB.StringSetAsync(RedisConstant.UI_MODEL_KEY, JsonSerializer.Serialize(input));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(AddOrUpdateCurrentUIAsync)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<string> GetFeeProcessing()
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(GetFeeProcessing)} method...");
                var result = await _redisDB.StringGetAsync(AppConstant.REDIS_KEY_FUSION_PROCESSING);

                return result.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(GetFeeProcessing)} method. Details: {ex.Message}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        #region Private method
        public async Task<List<WaitingVehicleModel>> GetWaitingVehicles()
        {
            var fusionObjects = await HashGetListAsync<FusionModel>(null, RedisConstant.SORTED_SET_FUSION_OUT);
            List<WaitingVehicleModel> waitingVehicles = new List<WaitingVehicleModel>();
            if (fusionObjects != null && fusionObjects.Any())
            {
                foreach (var fusion in fusionObjects)
                {
                    string? plateNumber = fusion.ANPRCam1;
                    long? inEpoch = null;

                    if (!string.IsNullOrEmpty(fusion.ANPRCam1))
                    {
                        var camData = _redisDB.StringGet(RedisConstant.StringType_CameraInKey(fusion.ANPRCam1)).ToString();
                        if (!string.IsNullOrEmpty(camData))
                        {
                            var anprCamValue = JsonSerializer.Deserialize<ANPRCameraModel>(camData);
                            inEpoch = anprCamValue?.CheckpointTimeEpoch;
                        }
                    }

                    if (!string.IsNullOrEmpty(fusion.RFID))
                    {
                        var rfidIn = _redisDB.StringGet(RedisConstant.StringType_RFIDInKey(fusion.RFID)).ToString();
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
        private async Task<List<T>?> HashGetListAsync<T>(Func<T, bool>? action, string sortedKey, Order order = Order.Ascending)
        {
            _logger.LogInformation($"Executing {nameof(HashGetListAsync)} method...");
            List<T>? result = new List<T>();

            try
            {
                var members = await _redisDB.SortedSetRangeByScoreWithScoresAsync(sortedKey, order: order);

                foreach (var member in members)
                {
                    var hashEntries = await _redisDB.HashGetAllAsync(member.Element.ToString());

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
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }
        #endregion
    }
}
