using EPAY.ETC.Core.API.Core.Models.Common;
using EPAY.ETC.Core.API.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.Vehicles
{
    public interface IVehicleHistoryService
    {
        Task<ValidationResult<VehicleHistoryModel>> Addsync(VehicleHistoryModel model);

    }
}
