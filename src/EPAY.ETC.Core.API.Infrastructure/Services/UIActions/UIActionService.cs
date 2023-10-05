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
using EPAY.ETC.Core.Models.Devices;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Fees;
using EPAY.ETC.Core.Models.Receipt.SessionReports;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using EPAY.ETC.Core.Publisher.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using StackExchange.Redis;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text.Json;
using static System.Net.WebRequestMethods;
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

        public UIActionService(ILogger<UIActionService> logger,
                               IPaymentStatusRepository paymentStatusRepository,
                               IAppConfigRepository appConfigRepository,
                               ICustomVehicleTypeRepository customVehicleTypeRepository,
                               IManualBarrierControlRepository manualBarrierControlRepository,
                               IDatabase redisDB)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _paymentStatusRepository = paymentStatusRepository ?? throw new ArgumentNullException(nameof(paymentStatusRepository));
            _appConfigRepository = appConfigRepository ?? throw new ArgumentNullException(nameof(appConfigRepository));
            _customVehicleTypeRepository = customVehicleTypeRepository ?? throw new ArgumentNullException(nameof(customVehicleTypeRepository));
            _manualBarrierControlRepository = manualBarrierControlRepository ?? throw new ArgumentNullException(nameof(manualBarrierControlRepository));
            _redisDB = redisDB ?? throw new ArgumentNullException(nameof(redisDB));
        }

        public async Task<ValidationResult<FeeModel?>> ReconcileVehicleInfoAsync(ReconcileVehicleInfoModel reconcileVehicleInfo)
        {
            _logger.LogInformation($"Executing {nameof(ReconcileVehicleInfoAsync)} method...");

            try
            {
                // Get FeeModel object from Redis.
                FeeModel feeModel = null;
                var feeObject = await _redisDB.StringGetAsync(RedisConstant.StringType_FeeModules(reconcileVehicleInfo?.ObjectId.ToString() ?? string.Empty));
                if(!string.IsNullOrEmpty(feeObject.ToString()))
                {
                    feeModel = JsonSerializer.Deserialize<FeeModel>(feeObject.ToString());
                    if(feeModel != null)
                    {
                        // EmployeeId
                        feeModel.EmployeeId = !string.IsNullOrEmpty(reconcileVehicleInfo?.EmployeeId) ? reconcileVehicleInfo?.EmployeeId : feeModel.EmployeeId;

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
                        feeModel.LaneInVehicle.LaneInId = !string.IsNullOrEmpty(reconcileVehicleInfo?.Vehicle?.LandIn?.LandId)
                           ? reconcileVehicleInfo?.Vehicle?.LandIn?.LandId
                           : feeModel.LaneInVehicle.LaneInId;
                        feeModel.LaneInVehicle.Epoch = (reconcileVehicleInfo?.Vehicle?.LandIn?.LaneDateTimeEpoch > 0)
                           ? reconcileVehicleInfo.Vehicle.LandIn.LaneDateTimeEpoch
                           : feeModel.LaneInVehicle.Epoch;
                        feeModel.LaneInVehicle.VehicleInfo.VehiclePhotoUrl = !string.IsNullOrEmpty(reconcileVehicleInfo?.Vehicle?.LandIn?.LanePhotoUrl)
                            ? reconcileVehicleInfo?.Vehicle?.LandIn?.LanePhotoUrl
                            : feeModel.LaneInVehicle.VehicleInfo.VehiclePhotoUrl;

                        // LandOut
                        feeModel.LaneOutVehicle.LaneOutId = !string.IsNullOrEmpty(reconcileVehicleInfo?.Vehicle?.LandOut.LandId)
                           ? reconcileVehicleInfo?.Vehicle?.LandOut?.LandId
                           : feeModel.LaneOutVehicle.LaneOutId;
                        feeModel.LaneOutVehicle.Epoch = (reconcileVehicleInfo?.Vehicle?.LandOut?.LaneDateTimeEpoch > 0)
                           ? reconcileVehicleInfo.Vehicle.LandOut.LaneDateTimeEpoch
                           : feeModel.LaneOutVehicle.Epoch;
                        feeModel.LaneOutVehicle.VehicleInfo.VehiclePhotoUrl = !string.IsNullOrEmpty(reconcileVehicleInfo?.Vehicle?.LandOut?.LanePhotoUrl)
                            ? reconcileVehicleInfo?.Vehicle?.LandOut?.LanePhotoUrl
                            : feeModel.LaneOutVehicle.VehicleInfo.VehiclePhotoUrl;

                        return ValidationResult.Success(feeModel);
                    }
                    else
                    {
                        _logger.LogError($"Fee object from Redis is null {nameof(ReconcileVehicleInfoAsync)} method.");
                       return ValidationResult.Success(feeModel);
                    }
                }
                else
                {
                    _logger.LogError($"Fee object from Redis is null {nameof(ReconcileVehicleInfoAsync)} method.");
                   return ValidationResult.Success(feeModel);
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
                    Limit = request.Limit
                };

                await _redisDB.HashSetAsync(Models.Constants.Constant.HASH_BARRIER_OPEN_STATUS, result.ToHashEntries());
                await _manualBarrierControlRepository.AddAsync(new Core.Models.ManualBarrierControl.ManualBarrierControlModel()
                {
                    Action = request.Action,
                    CreatedDate = DateTime.Now,
                    EmployeeId = request.EmployeeId,
                    LaneOutId = request.LaneId,
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
                LaneSessionReportModel result = new LaneSessionReportModel()
                {
                    PrintType = Models.Enums.ReceiptTypeEnum.SessionReport,
                    Layout = new LaneSessionLayoutModel()
                    {
                        Header = new Models.Receipt.HeaderModel
                        {
                            Heading = appConfig?.HeaderHeading?.ToUpper(),
                            SubHeading = appConfig?.HeaderSubHeading?.ToUpper(),
                            Line1 = appConfig?.HeaderLine1,
                            Line2 = appConfig?.HeaderLine2
                        },
                        Footer = new Models.Receipt.FooterModel()
                        {
                            Line1 = $"{appConfig?.FooterLine1?.Trim()} Ngày {request.ToDate.Day} tháng {request.ToDate.Month} năm {request.ToDate.Year}",
                            Line2 = $"{appConfig?.FooterLine2?.Trim()}"
                        },
                        Body = new LaneSessionBodyModel()
                    }
                };

                var firstPaymentStatus = paymentStatuses.Where(x => x.Payment != null && x.Payment.Fee != null).FirstOrDefault();

                // TODO: Need to get fullname of employee
                result.Layout.Footer.Line3 = "Nguyen Van A";

                result.Layout.Body.Heading = Models.Enums.ReceiptTypeEnum.SessionReport.ToEnumMemberAttrValue().ToUpper();

                // TODO: Need to get name of Shift
                result.Layout.Body.SubHeading1 = $"Ngày: {request.FromDate.ToString("dd/MM/yyyy")}  Ca: 01  Trạm: {firstPaymentStatus?.Payment.LaneOutId}";
                result.Layout.Body.SubHeading2 = $"Từ giờ: {request.FromDate.ToString("HH:mm:ss")}  Đến giờ: {request.ToDate.ToString("HH:mm:ss")}";

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
                    PaymentStatus = new PaymentStatusModel()
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

        public Task<ValidationResult<Models.UI.UIModel>> LoadCurrentUIAsync()
        {
            throw new NotImplementedException();
        }
    }
}
