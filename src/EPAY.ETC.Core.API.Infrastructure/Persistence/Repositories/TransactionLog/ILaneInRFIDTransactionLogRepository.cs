using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.TransactionLog;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TransactionLog
{
    public interface ILaneInRFIDTransactionLogRepository : IAddRepository<LaneInRFIDTransactionLog, Guid>
        , IGetAllRepository<LaneInRFIDTransactionLog, Guid>
        , IUpdateRepository<LaneInRFIDTransactionLog, Guid>
    {
    }
}
