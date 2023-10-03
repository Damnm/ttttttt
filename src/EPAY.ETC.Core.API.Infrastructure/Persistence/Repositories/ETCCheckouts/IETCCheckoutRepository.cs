using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.ETCCheckOuts;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ETCCheckouts
{
    public interface IETCCheckoutRepository : IRepository<ETCCheckoutDataModel, Guid>
    {
    }
}
