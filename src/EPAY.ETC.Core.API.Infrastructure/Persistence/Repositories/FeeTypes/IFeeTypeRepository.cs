using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.FeeTypes;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.FeeTypes
{
    public interface IFeeTypeRepository : IGetAllRepository<FeeTypeModel, Guid>
    {
    }
}
