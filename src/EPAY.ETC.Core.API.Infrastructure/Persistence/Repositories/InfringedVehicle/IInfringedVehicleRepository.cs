using EPAY.ETC.Core.API.Core.Models.InfringeredVehicle;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.InfringedVehicle
{
    public interface IInfringedVehicleRepository
    {
        Task<List<InfringedVehicleModel>> GetInfringedVehicleAsync(Expression<Func<InfringedVehicleModel, bool>>? expression = null);
    }
}
