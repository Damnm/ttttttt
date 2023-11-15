using EPAY.ETC.Core.API.Core.Models.PrintLog;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.PrintLog
{
    public interface IPrintLogService
    {
        Task<ValidationResult<PrintLogModel>> AddAsync(PrintLogRequestModel input);
        Task<ValidationResult<PrintLogModel>> GetByIdAsync(Guid id);
        Task<ValidationResult<PrintLogModel>> UpdateAsync(Guid id , PrintLogRequestModel input);
        Task<ValidationResult<PrintLogModel>> RemoveAsync(Guid id);
    }
}
