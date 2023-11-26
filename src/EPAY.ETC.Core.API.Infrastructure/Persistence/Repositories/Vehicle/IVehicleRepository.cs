using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Vehicle
{
    public interface IVehicleRepository : IRepository<VehicleModel, Guid>
    {
        Task<List<InfringedVehicleInfoModel>> GetVehicleWithInfringementAsync(Expression<Func<VehicleModel, bool>> expression, bool isRFID);
    }
}
