using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.InfringeredVehicle;
using EPAY.ETC.Core.API.Core.Models.TicketType;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.InfringedVehicle
{
    public interface IInfringedVehicleRepository: IGetAllRepository<InfringedVehicleModel, Guid>
    {
    }
}
