using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.Models.Validation;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.Vehicles
{
    public interface IVehicleService
    {
        Task<ValidationResult<VehicleModel>> AddAsync(VehicleRequestModel input);
        Task<ValidationResult<VehicleModel>> GetByIdAsync(Guid id);
        Task<ValidationResult<VehicleModel>> UpdateAsync(Guid id ,VehicleRequestModel input);
        Task<ValidationResult<VehicleModel>> RemoveAsync(Guid id);
    }
}
