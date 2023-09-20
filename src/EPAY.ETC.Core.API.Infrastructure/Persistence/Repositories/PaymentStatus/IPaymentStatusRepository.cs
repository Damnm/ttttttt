using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.API.Core.Models.PaymentStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PaymentStatus
{
    public interface IPaymentStatusRepository: IRepository<Core.Models.PaymentStatus.PaymentStatusModel, Guid>
    {
    }
}
