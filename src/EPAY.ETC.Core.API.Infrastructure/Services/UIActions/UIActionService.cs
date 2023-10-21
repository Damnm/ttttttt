using EPAY.ETC.Core.API.Core.Extensions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.API.Core.Models.Configs;
using EPAY.ETC.Core.API.Core.Models.Vehicle.ReconcileVehicle;
using EPAY.ETC.Core.API.Core.Utils;
using EPAY.ETC.Core.API.Infrastructure.Common.Extensions;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.CustomVehicleTypes;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ETCCheckouts;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ManualBarrierControls;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PaymentStatus;
using EPAY.ETC.Core.Models.BarrierOpenStatus;
using EPAY.ETC.Core.Models.Constants;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Fees;
using EPAY.ETC.Core.Models.Receipt.SessionReports;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.UI;
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
        private readonly IPaymentStatusRepository _paymentStatusRepository;
        private readonly IAppConfigRepository _appConfigRepository;
        private readonly ICustomVehicleTypeRepository _customVehicleTypeRepository;
        private readonly IManualBarrierControlRepository _manualBarrierControlRepository;
        private readonly IDatabase _redisDB;
        private readonly IOptions<UIModel> _uiOptions;

        public UIActionService(ILogger<UIActionService> logger,
                               IPaymentStatusRepository paymentStatusRepository,
                               IAppConfigRepository appConfigRepository,
                               ICustomVehicleTypeRepository customVehicleTypeRepository,
                               IManualBarrierControlRepository manualBarrierControlRepository,
                               IDatabase redisDB,
                               IOptions<UIModel> uiOptions)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _paymentStatusRepository = paymentStatusRepository ?? throw new ArgumentNullException(nameof(paymentStatusRepository));
            _appConfigRepository = appConfigRepository ?? throw new ArgumentNullException(nameof(appConfigRepository));
            _customVehicleTypeRepository = customVehicleTypeRepository ?? throw new ArgumentNullException(nameof(customVehicleTypeRepository));
            _manualBarrierControlRepository = manualBarrierControlRepository ?? throw new ArgumentNullException(nameof(manualBarrierControlRepository));
            _redisDB = redisDB ?? throw new ArgumentNullException(nameof(redisDB));
            _uiOptions = uiOptions ?? throw new ArgumentNullException(nameof(uiOptions));
        }

        public async Task<ValidationResult<ReconcileResultModel>> ReconcileVehicleInfoAsync(ReconcileVehicleInfoModel reconcileVehicleInfo)
        {
            _logger.LogInformation($"Executing {nameof(ReconcileVehicleInfoAsync)} method...");

            try
            {
                ReconcileResultModel result = new ReconcileResultModel();
                if (reconcileVehicleInfo?.Payment != null)
                {
                    var uiModelStr = (string?)await _redisDB.StringGetAsync(RedisConstant.UI_MODEL_KEY);
                    UIModel? uiModel = null;
                    if (!string.IsNullOrEmpty(uiModelStr))
                        uiModel = JsonSerializer.Deserialize<UIModel>(uiModelStr);

                    result.PaymentStatus = new PaymenStatusResponseModel()
                    {
                        ObjectId = reconcileVehicleInfo.ObjectId ?? Guid.NewGuid(),
                        PaymentStatus = new ETC.Core.Models.Fees.PaymentStatusModel()
                        {
                            PaymentId = reconcileVehicleInfo.Payment.PaymentId,
                            Status = reconcileVehicleInfo.Payment?.PaymentStatus ?? PaymentStatusEnum.Unpaid,
                            PaymentMethod = reconcileVehicleInfo.Payment?.PaymentType ?? PaymentMethodEnum.Cash,
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
                    var feeObject = await _redisDB.StringGetAsync(RedisConstant.StringType_FeeModules(reconcileVehicleInfo?.ObjectId.ToString() ?? string.Empty));

                    if (!string.IsNullOrEmpty(feeObject.ToString()))
                    {
                        feeModel = JsonSerializer.Deserialize<FeeModel>(feeObject.ToString());
                        if (feeModel != null)
                        {
                            // EmployeeId
                            feeModel.EmployeeId = !string.IsNullOrEmpty(reconcileVehicleInfo?.EmployeeId) ? reconcileVehicleInfo?.EmployeeId : feeModel.EmployeeId;

                            if (feeModel.LaneInVehicle == null)
                                feeModel.LaneInVehicle = new LaneInVehicleModel();
                            if (feeModel.LaneInVehicle.VehicleInfo == null)
                                feeModel.LaneInVehicle.VehicleInfo = new ETC.Core.Models.VehicleInfoModel();
                            if (feeModel.LaneOutVehicle == null)
                                feeModel.LaneOutVehicle = new LaneOutVehicleModel();
                            if (feeModel.LaneOutVehicle.VehicleInfo == null)
                                feeModel.LaneOutVehicle.VehicleInfo = new ETC.Core.Models.VehicleInfoModel();

                            // Platenumber
                            feeModel.LaneInVehicle.VehicleInfo.PlateNumber = !string.IsNullOrEmpty(reconcileVehicleInfo?.Vehicle?.PlateNumber)
                                ? reconcileVehicleInfo?.Vehicle?.PlateNumber
                                : feeModel.LaneInVehicle.VehicleInfo.PlateNumber;
                            feeModel.LaneOutVehicle.VehicleInfo.PlateNumber = !string.IsNullOrEmpty(reconcileVehicleInfo?.Vehicle?.PlateNumber)
                                ? reconcileVehicleInfo?.Vehicle?.PlateNumber
                                : feeModel.LaneOutVehicle.VehicleInfo.PlateNumber;

                            // RFID
                            feeModel.LaneInVehicle.RFID = !string.IsNullOrEmpty(reconcileVehicleInfo?.Vehicle?.RFID)
                                ? reconcileVehicleInfo?.Vehicle?.PlateNumber
                                : feeModel.LaneInVehicle.RFID;
                            feeModel.LaneOutVehicle.RFID = !string.IsNullOrEmpty(reconcileVehicleInfo?.Vehicle?.RFID)
                                ? reconcileVehicleInfo?.Vehicle?.PlateNumber
                                : feeModel.LaneOutVehicle.RFID;

                            // VehicleType
                            feeModel.LaneInVehicle.VehicleInfo.VehicleType = !string.IsNullOrEmpty(reconcileVehicleInfo?.Vehicle?.VehicleType)
                                ? reconcileVehicleInfo?.Vehicle?.VehicleType
                                : feeModel.LaneInVehicle.VehicleInfo.VehicleType;

                            feeModel.LaneOutVehicle.VehicleInfo.VehicleType = !string.IsNullOrEmpty(reconcileVehicleInfo?.Vehicle?.VehicleType)
                               ? reconcileVehicleInfo?.Vehicle?.VehicleType
                               : feeModel.LaneOutVehicle.VehicleInfo.VehicleType;

                            // LandIn
                            feeModel.LaneInVehicle.LaneInId = !string.IsNullOrEmpty(reconcileVehicleInfo?.Vehicle?.In?.LaneInId)
                               ? reconcileVehicleInfo?.Vehicle?.In?.LaneInId
                               : feeModel.LaneInVehicle.LaneInId;
                            feeModel.LaneInVehicle.Epoch = (reconcileVehicleInfo?.Vehicle?.In?.LaneInDateTimeEpoch > 0)
                               ? reconcileVehicleInfo?.Vehicle?.In?.LaneInDateTimeEpoch ?? 0
                               : feeModel.LaneInVehicle.Epoch;
                            feeModel.LaneInVehicle.VehicleInfo.VehiclePhotoUrl = !string.IsNullOrEmpty(reconcileVehicleInfo?.Vehicle?.In?.LaneInPhotoUrl)
                                ? reconcileVehicleInfo?.Vehicle?.In?.LaneInPhotoUrl
                                : feeModel.LaneInVehicle.VehicleInfo.VehiclePhotoUrl;

                            // LandOut
                            feeModel.LaneOutVehicle.LaneOutId = !string.IsNullOrEmpty(reconcileVehicleInfo?.Vehicle?.Out?.LaneOutId)
                               ? reconcileVehicleInfo?.Vehicle?.Out?.LaneOutId
                               : feeModel.LaneOutVehicle.LaneOutId;
                            feeModel.LaneOutVehicle.Epoch = (reconcileVehicleInfo?.Vehicle?.Out?.LaneOutDateTimeEpoch > 0)
                               ? reconcileVehicleInfo.Vehicle.Out.LaneOutDateTimeEpoch ?? 0
                               : feeModel.LaneOutVehicle.Epoch;
                            feeModel.LaneOutVehicle.VehicleInfo.VehiclePhotoUrl = !string.IsNullOrEmpty(reconcileVehicleInfo?.Vehicle?.Out?.LaneOutPhotoUrl)
                                ? reconcileVehicleInfo?.Vehicle?.Out?.LaneOutPhotoUrl
                                : feeModel.LaneOutVehicle.VehicleInfo.VehiclePhotoUrl;

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

        public async Task<ValidationResult<BarrierOpenStatus>> ManipulateBarrier(BarrierRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(ManipulateBarrier)} method...");

                BarrierOpenStatus result = new BarrierOpenStatus()
                {
                    Status = request.Action!,
                    Limit = request.Limit ?? 0
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
                    BottomLine = ConvertUtil.DocTienBangChuV2(grandTotal)
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
                var uiModelStr = await _redisDB.StringGetAsync(RedisConstant.UI_MODEL_KEY);

                UIModel? result = null;

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

                    if (result.Header == null)
                        result.Header = new HeaderModel();
                    result.Header.EmployeeName = $"{authenticatedEmployee.FirstName} {authenticatedEmployee.LastName}";
                }

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
    }
}
