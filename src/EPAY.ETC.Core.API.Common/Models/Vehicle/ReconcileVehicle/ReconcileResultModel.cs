using EPAY.ETC.Core.Models.Fees;

namespace EPAY.ETC.Core.API.Core.Models.Vehicle.ReconcileVehicle
{
    public class ReconcileResultModel
    {
        public FeeModel? Fee { set; get; }
        public PaymenStatusResponseModel? PaymentStatus { set; get; }
    }
}
