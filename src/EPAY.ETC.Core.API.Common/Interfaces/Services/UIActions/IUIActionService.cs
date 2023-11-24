using EPAY.ETC.Core.API.Core.Models.UI;
using EPAY.ETC.Core.API.Core.Models.Vehicle.ReconcileVehicle;
using EPAY.ETC.Core.Models.Fees;
using EPAY.ETC.Core.Models.Receipt.SessionReports;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.UI;
using EPAY.ETC.Core.Models.Validation;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions
{
    public interface IUIActionService
    {
        ValidationResult<ReconcileResultModel> ReconcileVehicleInfo(ReconcileVehicleInfoModel reconcileVehicleInfo);
        Task<ValidationResult<LaneSessionReportModel>> PrintLaneSessionReportAsync(LaneSessionReportRequestModel request);
        ValidationResult<PaymenStatusResponseModel> UpdatePaymentMethod(PaymentStatusUIRequestModel request);
        Task<ValidationResult<ManipulateBarrierResponseModel>> ManipulateBarrierAsync(BarrierRequestModel request);
        Task<ValidationResult<UIModel>> LoadCurrentUIAsync(AuthenticatedEmployeeResponseModel? authenticatedEmployee = null);
        void AddOrUpdateCurrentUI(UIModel input);
        string GetFeeProcessing();
    }
}
