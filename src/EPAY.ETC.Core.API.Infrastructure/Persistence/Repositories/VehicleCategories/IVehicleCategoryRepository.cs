using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.VehicleCategories;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.VehicleCategories
{
    public interface IVehicleCategoryRepository : IGetAllRepository<VehicleCategoryModel, Guid>
    {
    }
}
