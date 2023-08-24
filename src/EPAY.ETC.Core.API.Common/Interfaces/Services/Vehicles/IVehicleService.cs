using EPAY.ETC.Core.API.Core.Models.Common;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.API.Core.Validation;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.Vehicles
{
    public interface IVehicleService
    {
        Task<ValidationResult<VehicleModel>> AddAsync(VehicleModel input);
        Task<ValidationResult<VehicleModel>> GetByIdAsync(Guid id);
        Task<ValidationResult<VehicleModel>> UpdateAsync(VehicleRequestModel input);
        Task<ValidationResult<Guid>> RemoveAsync(Guid id);
        //Task<ValidationResult<VehicleSearchResponseModel>> SearchAsync(string stationId, VehicleSearchRequestModel input);
    }
}
