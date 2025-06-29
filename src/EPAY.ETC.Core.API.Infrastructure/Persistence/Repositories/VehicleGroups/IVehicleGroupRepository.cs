﻿using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.VehicleGroups;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.VehicleGroups
{
    public interface IVehicleGroupRepository : IGetAllRepository<VehicleGroupModel, Guid>
    {
    }
}
