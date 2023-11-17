using EPAY.ETC.Core.Models.BarrierOpenStatus;
using EPAY.ETC.Core.Models.Fees;
using EPAY.ETC.Core.Models.UI;

namespace EPAY.ETC.Core.API.Core.Models.UI
{
    public class ManipulateBarrierResponseModel
    {
        public PaymentModel? Payment { set; get; }
        public FeeModel? Fee { set; get; }
        public BarrierOpenStatus BarrierOpenStatus { set; get; }
        public UIModel? UI { set; get; }
    }
}
