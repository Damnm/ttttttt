using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.Payment
{
    public interface IPaymentService
    {
        public Task<ValidationResult<Models.Payment.PaymentModel>> AddAsync(PaymentAddRequestModel input);
        public Task<ValidationResult<Models.Payment.PaymentModel>> GetByIdAsync(Guid id);
        public Task<ValidationResult<Models.Payment.PaymentModel>> UpdateAsync(Guid id, PaymentUpdateRequestModel request);
        public Task<ValidationResult<Models.Payment.PaymentModel>> RemoveAsync(Guid id);
    }
}
