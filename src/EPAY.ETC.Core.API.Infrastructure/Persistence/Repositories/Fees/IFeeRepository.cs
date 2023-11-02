using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using FeeModel = EPAY.ETC.Core.API.Core.Models.Fees.FeeModel;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Fees
{
    public interface IFeeRepository : IRepository<FeeModel, Guid>
    {
        Task<FeeModel?> GetByObjectIdAsync(Guid objectId);
    }
}
