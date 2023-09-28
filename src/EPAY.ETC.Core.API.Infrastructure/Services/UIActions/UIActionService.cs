using EPAY.ETC.Core.API.Core.Extensions;
using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.API.Core.Models.Configs;
using EPAY.ETC.Core.API.Core.Utils;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.CustomVehicleTypes;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ETCCheckouts;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PaymentStatus;
using EPAY.ETC.Core.Models.Devices;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Models.Fees;
using EPAY.ETC.Core.Models.Receipt.SessionReports;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Services.UIActions
{
    public class UIActionService : IUIActionService
    {
        private readonly ILogger<UIActionService> _logger;
        private readonly IPaymentStatusRepository _paymentStatusRepository;
        private readonly IAppConfigRepository _appConfigRepository;
        private readonly ICustomVehicleTypeRepository _customVehicleTypeRepository;

        public UIActionService(ILogger<UIActionService> logger,
                               IPaymentStatusRepository paymentStatusRepository,
                               IAppConfigRepository appConfigRepository,
                               ICustomVehicleTypeRepository customVehicleTypeRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _paymentStatusRepository = paymentStatusRepository ?? throw new ArgumentNullException(nameof(paymentStatusRepository));
            _appConfigRepository = appConfigRepository ?? throw new ArgumentNullException(nameof(appConfigRepository));
            _customVehicleTypeRepository = customVehicleTypeRepository;
        }

        public Task CreateDataInput()
        {
            throw new NotImplementedException();
        }

        public Task<ValidationResult<BarrierModel>> ManipulateBarrier(BarrierModel request)
        {
            throw new NotImplementedException();
        }

        public async Task<ValidationResult<SessionReportModel>> PrintLaneSessionReport(SessionReportRequestModel request)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(PrintLaneSessionReport)} method...");

                Expression<Func<AppConfigModel, bool>> expression = s => s.IsApply == true;

                var appConfig = (await _appConfigRepository.GetAllAsync(expression)).FirstOrDefault();
                var vehicleTypes = await _customVehicleTypeRepository.GetAllAsync();
                vehicleTypes = vehicleTypes.ToList().Where(x => !string.IsNullOrEmpty(x.Name.GetDescription())).OrderBy(x => x.Name);

                var paymentStatuses = await _paymentStatusRepository.GetAllWithNavigationAsync(request);
                SessionReportModel result = new SessionReportModel()
                {
                    PrintType = Models.Enums.ReceiptTypeEnum.SessionReport,
                    Layout = new SessionLayoutModel()
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
                        Body = new SessionBodyModel()
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
                result.Layout.Body.Data = new SessionDataModel()
                {
                    Qty = paymentStatuses.Count(),
                    GrandTotal = grandTotal,
                    BottomLine = ConvertUtil.DocTienBangChuV2(grandTotal)
                };

                var paymentMethodCashs = paymentStatuses
                    .Where(x => x.PaymentMethod == PaymentMethodEnum.Cash);

                var paymentMethodNoneCashs = paymentStatuses
                    .Where(x => x.PaymentMethod != PaymentMethodEnum.Cash);

                result.Layout.Body.Data.Payments = new List<SessionPaymentDataModel>()
                {
                    new SessionPaymentDataModel()
                    {
                        Name = $"Hình thức thu phí - Tiền mặt",
                        Qty = paymentMethodCashs.Count(),
                        Total = paymentMethodCashs.Sum(p => p.Amount),
                        Details = vehicleTypes.Select(s =>
                        {
                            var subPayments = paymentMethodCashs.Where(x => x.Payment?.CustomVehicleTypeId == s.Id);

                            return new SessionPaymentDetailDataModel()
                            {
                                Type = s.Name.GetDescription(),
                                Qty = subPayments.Count(),
                                Subtotal = subPayments.Sum(p => p.Amount)
                            };
                        }).ToList()
                    },
                    new SessionPaymentDataModel()
                    {
                        Name = $"Hình thức thu phí - Phi tiền mặt",
                        Qty = paymentMethodNoneCashs.Count(),
                        Total = paymentMethodNoneCashs.Sum(p => p.Amount),
                        Details = vehicleTypes.Select(s =>
                        {
                            var subPayments = paymentMethodNoneCashs.Where(x => x.Payment?.CustomVehicleTypeId == s.Id);

                            return new SessionPaymentDetailDataModel()
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
            throw new NotImplementedException();
        }
    }
}
