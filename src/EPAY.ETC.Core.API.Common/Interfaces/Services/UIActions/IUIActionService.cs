using EPAY.ETC.Core.Models.Devices;
using EPAY.ETC.Core.Models.Fees;
using EPAY.ETC.Core.Models.Receipt.SessionReports;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions
{
    public interface IUIActionService
    {
        Task<SessionReceiptModel> PrintLaneSessionReport(SessionReceiptRequestModel input);
        Task CreateDataInput();
        Task<PaymenStatusResponseModel> UpdatePaymentMethod(PaymentStatusModel input);
        Task<BarrierModel> ManipulateBarrier(BarrierModel input);
    }
}
