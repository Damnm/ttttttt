using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.FeeVehicleCategories;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.FeeVehicleCategories
{
    public interface IFeeVehicleCategoryRepository : IGetAllRepository<FeeVehicleCategoryModel, Guid>
    {
    }
}
