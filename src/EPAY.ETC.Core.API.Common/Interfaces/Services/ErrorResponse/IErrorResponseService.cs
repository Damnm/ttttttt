using EPAY.ETC.Core.API.Core.Models.ErrorResponse;
using EPAY.ETC.Core.Models.Validation;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.ErrorResponse
{
    public interface IErrorResponseService
    {
        public Task<ValidationResult<IEnumerable<ErrorResponseModel>>> GetAllAsync(Expression<Func<ErrorResponseModel, bool>>? expressison = null);
    }
}
