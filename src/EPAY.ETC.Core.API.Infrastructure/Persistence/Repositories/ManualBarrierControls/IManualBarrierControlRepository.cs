using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.ManualBarrierControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ManualBarrierControls
{
    public interface IManualBarrierControlRepository : IRepository<ManualBarrierControlModel, Guid>
    {

    }
}
