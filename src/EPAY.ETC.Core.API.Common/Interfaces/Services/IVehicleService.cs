using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Core.Validation;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services
{
    public interface IVehicleService
    {
        Task<ValidationResult<VehicleModel>> AddAsync(VehicleModel entity);
        Task<ValidationResult<VehicleModel>> GetByIdAsync(Guid id);
        Task<ValidationResult<IEnumerable<VehicleModel>>> GetAllVehicleAsync();
        Task<ValidationResult<VehicleModel>> UpdateAsync(Guid id, VehicleModel updatedVehicle);
    }
}
