using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.Vehicle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories
{
    public interface IVehicleRepository: IAddRepository<VehicleModel, Guid>
    {
    }
}
