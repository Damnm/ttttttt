using EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions;
using EPAY.ETC.Core.Models.Devices;
using EPAY.ETC.Core.Models.Fees;
using EPAY.ETC.Core.Models.Receipt.SessionReports;
using EPAY.ETC.Core.Models.Request;

namespace EPAY.ETC.Core.API.Infrastructure.Services.UIActions
{
    public class UIActionService : IUIActionService
    {
        public Task CreateDataInput()
        {
            throw new NotImplementedException();
        }

        public Task<BarrierModel> ManipulateBarrier(BarrierModel input)
        {
            throw new NotImplementedException();
        }

        public Task<SessionReceiptModel> PrintLaneSessionReport(SessionReceiptRequestModel input)
        {
            throw new NotImplementedException();
        }

        public Task<PaymenStatusResponseModel> UpdatePaymentMethod(PaymentStatusUIRequestModel input)
        {
            throw new NotImplementedException();
        }
    }
}
