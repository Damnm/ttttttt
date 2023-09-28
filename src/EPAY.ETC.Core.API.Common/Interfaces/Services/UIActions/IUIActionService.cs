using EPAY.ETC.Core.Models.Devices;
using EPAY.ETC.Core.Models.Fees;
using EPAY.ETC.Core.Models.Receipt.SessionReports;
using EPAY.ETC.Core.Models.Request;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions
{
    public interface IUIActionService
    {
        Task<SessionReportModel> PrintLaneSessionReport(SessionReportRequestModel request);
        Task CreateDataInput();
        Task<PaymenStatusResponseModel> UpdatePaymentMethod(PaymentStatusUIRequestModel request);
        Task<BarrierModel> ManipulateBarrier(BarrierModel request);
    }
}
