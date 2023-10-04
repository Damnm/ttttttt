using EPAY.ETC.Core.Models.Fees.PaymentStatusHistory;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.PaymentStatus
{
    public interface IPaymentStatusService
    {
        public Task<ValidationResult<Models.PaymentStatus.PaymentStatusModel>> AddAsync(PaymentStatusAddRequestModel input);
        public Task<ValidationResult<Models.PaymentStatus.PaymentStatusModel>> GetByIdAsync(Guid id);
        public Task<ValidationResult<Models.PaymentStatus.PaymentStatusModel>> UpdateAsync(Guid id, PaymentStatusUpdateRequestModel request);
        public Task<ValidationResult<Models.PaymentStatus.PaymentStatusModel>> RemoveAsync(Guid id);
        public Task<ValidationResult<PaymentStatusHistoryModel>> GetPaymentStatusHistoryAsync(Guid paymentId);
    }
}
