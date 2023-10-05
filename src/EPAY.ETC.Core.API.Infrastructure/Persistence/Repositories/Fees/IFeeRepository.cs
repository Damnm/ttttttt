using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.Fees;
using EPAY.ETC.Core.Models.Fees;
using EPAY.ETC.Core.Models.Validation;
using FeeModel = EPAY.ETC.Core.API.Core.Models.Fees.FeeModel;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Fees
{
    public interface IFeeRepository : IRepository<FeeModel, Guid>
    {
        Task<FeeModel?> GetByObjectIdAsync(Guid objectId);
    }
}
