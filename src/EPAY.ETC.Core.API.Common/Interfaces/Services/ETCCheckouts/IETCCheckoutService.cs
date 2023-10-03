using EPAY.ETC.Core.API.Core.Models.ETCCheckOuts;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.ETCCheckouts
{
    public interface IETCCheckoutService
    {
        public Task<ValidationResult<ETCCheckoutResponseModel>> AddAsync(ETCCheckoutAddUpdateRequestModel input);
        public Task<ValidationResult<ETCCheckoutResponseModel?>> GetByIdAsync(Guid id);
        public Task<ValidationResult<IEnumerable<ETCCheckoutResponseModel>>> GetAllAsync(Expression<Func<ETCCheckoutDataModel, bool>>? expressison = null);
        public Task<ValidationResult<ETCCheckoutResponseModel>> UpdateAsync(Guid id, ETCCheckoutAddUpdateRequestModel request);
        public Task<ValidationResult<ETCCheckoutResponseModel?>> RemoveAsync(Guid id);
    }
}
