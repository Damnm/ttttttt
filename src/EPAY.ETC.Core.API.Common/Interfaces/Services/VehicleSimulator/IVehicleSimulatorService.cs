using EPAY.ETC.Core.API.Core.Models.Simulator;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using EPAY.ETC.Core.Models.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.VehicleSimulator
{
    public interface IVehicleSimulatorService
    {
        Task<ValidationResult<VehicleSimulatorModel>> AddAsync(VehicleSimulatorRequestModel input);
        Task<ValidationResult<VehicleSimulatorModel>> GetByIdAsync(Guid id);
        Task<ValidationResult<VehicleSimulatorModel>> UpdateAsync(Guid id, VehicleSimulatorRequestModel input);
        Task<ValidationResult<VehicleSimulatorModel>> RemoveAsync(Guid id);
    }
}
