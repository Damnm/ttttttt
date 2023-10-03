using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.ETCCheckOuts;
using EPAY.ETC.Core.Models.Request;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ETCCheckouts
{
    public interface IETCCheckoutRepository : IRepository<ETCCheckoutDataModel, Guid>
    {
        Task<Core.DtoModels.ETCCheckOuts.ETCCheckoutFilterResultDto> GetAllByConditionAsync(ETCCheckoutFilterModel? filter = null);
    }
}
