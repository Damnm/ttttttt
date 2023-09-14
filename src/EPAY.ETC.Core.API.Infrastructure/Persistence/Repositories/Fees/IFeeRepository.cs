using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.Fees;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Fees
{
    public interface IFeeRepository : IRepository<FeeModel, Guid>
    {
    }
}
