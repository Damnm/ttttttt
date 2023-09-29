using EPAY.ETC.Core.Models.BarrierOpenStatus;
using EPAY.ETC.Core.Models.Fees;
using EPAY.ETC.Core.Models.Receipt.SessionReports;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions
{
    public interface IUIActionService
    {
        Task<ValidationResult<SessionReportModel>> PrintLaneSessionReport(SessionReportRequestModel request);
        Task CreateDataInput();
        Task<ValidationResult<PaymenStatusResponseModel>> UpdatePaymentMethod(PaymentStatusUIRequestModel request);
        Task<ValidationResult<BarrierOpenStatus>> ManipulateBarrier(BarrierRequestModel request);
    }
}
