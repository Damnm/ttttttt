using EPAY.ETC.Core.API.Core.Models.ErrorResponse;
using EPAY.ETC.Core.Models.Validation;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.ErrorResponse
{
    public interface IErrorResponseService
    {
        public Task<ValidationResult<List<ErrorResponseModel>>> GetErrorResponseBySourceAync(string source);
    }
}
