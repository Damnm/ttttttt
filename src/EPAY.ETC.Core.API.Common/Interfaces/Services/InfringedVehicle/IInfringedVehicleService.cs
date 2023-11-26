using EPAY.ETC.Core.API.Core.Models.InfringeredVehicle;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.Models.Validation;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.InfringedVehicle
{
    public interface IInfringedVehicleService
    {
        public Task<ValidationResult<List<InfringedVehicleInfoModel>>?> GetByRFIDOrPlateNumberAsync(string rfidOrPlateNumber);
    }
}
