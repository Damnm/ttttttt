using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.Configs;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ETCCheckouts
{
    public interface IAppConfigRepository : IGetAllRepository<AppConfigModel, Guid>
    {
    }
}
