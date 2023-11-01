using EPAY.ETC.Core.Models.Fees.PaidVehicleHistory;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.Payment
{
    public interface IPaymentService
    {
        public Task<ValidationResult<Models.Payment.PaymentModel>> AddAsync(PaymentAddOrUpdateRequestModel input);
        public Task<ValidationResult<Models.Payment.PaymentModel>> GetByIdAsync(Guid id);
        public Task<ValidationResult<Models.Payment.PaymentModel>> UpdateAsync(Guid id, PaymentAddOrUpdateRequestModel request);
        public Task<ValidationResult<Models.Payment.PaymentModel?>> RemoveAsync(Guid id);
        public Task<ValidationResult<List<PaidVehicleHistoryModel>>> GetPaidVehicleHistoryAsync(string? laneId = null);
    }
}
 