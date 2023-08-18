using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories
{
    public interface IVehicleRepository: IAddRepository<VehicleModel, Guid>
    {
        Task<VehicleModel?> GetByIdAsync(Guid id);
        Task<IEnumerable<VehicleModel>> GetAllAsync();
        Task<VehicleModel> UpdateAsync(VehicleModel entity);
    }
}
