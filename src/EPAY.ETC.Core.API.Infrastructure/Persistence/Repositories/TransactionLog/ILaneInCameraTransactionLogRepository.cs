using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.TransactionLog;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TransactionLog
{
    public interface ILaneInCameraTransactionLogRepository : IAddRepository<LaneInCameraTransactionLog, Guid>
        , IGetAllRepository<LaneInCameraTransactionLog, Guid>
        , IUpdateRepository<LaneInCameraTransactionLog, Guid>
    {
    }
}
