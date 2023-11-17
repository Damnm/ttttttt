using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.PrintLog;
using EPAY.ETC.Core.Models.Request;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PrintLog
{
    public interface IPrintLogRepository : IRepository<PrintLogModel, Guid>
    {
        Task<string?> PrintAsync(PrintRequestModel request);
    }
}
