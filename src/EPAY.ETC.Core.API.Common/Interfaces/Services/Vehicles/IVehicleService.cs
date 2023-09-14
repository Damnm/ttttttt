using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Core.Validation;
using System.Linq.Expressions;

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
