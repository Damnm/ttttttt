using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Core.Validation;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services
{
    public interface IVehicleService
    {
        Task<ValidationResult<VehicleModel>> AddAsync(VehicleModel entity);
    }
}
