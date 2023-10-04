using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.PaymentStatus;
using EPAY.ETC.Core.Models.Receipt.SessionReports;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PaymentStatus
{
    public interface IPaymentStatusRepository : IRepository<Core.Models.PaymentStatus.PaymentStatusModel, Guid>
    {
        Task<IEnumerable<PaymentStatusModel>> GetAllWithNavigationAsync(LaneSessionReportRequestModel request);
    }
}
