using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.CustomVehicleTypes;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.CustomVehicleTypes
{
    public interface ICustomVehicleTypeRepository : IGetAllRepository<CustomVehicleTypeModel, Guid>
    {
    }
}
