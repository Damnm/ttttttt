using EPAY.ETC.Core.API.Core.Models.InfringeredVehicle;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.InfringedVehicle
{
    public interface IInfringedVehicleService
    {
        Task<ValidationResult<List<InfringedVehicleInfoModel>>?> GetAllAsync(Expression<Func<InfringedVehicleModel, bool>>? expressison = null);
    }
}
