using EPAY.ETC.Core.API.Core.Models.TransactionLog;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.Transaction
{
    public interface ILaneInCameraTransactionLogService
    {
        Task<ValidationResult<bool>> UpdateInsertAsync(LaneInCameraTransactionLogRequest input);
    }
}
