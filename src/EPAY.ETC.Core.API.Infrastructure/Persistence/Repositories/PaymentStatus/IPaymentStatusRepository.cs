using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.PaymentStatus;
using EPAY.ETC.Core.Models.Receipt.SessionReports;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.PaymentStatus
{
    public interface IPaymentStatusRepository : IRepository<Core.Models.PaymentStatus.PaymentStatusModel, Guid>
    {
        Task<IEnumerable<PaymentStatusModel>> GetAllWithNavigationAsync(LaneSessionReportRequestModel request);
        Task<IQueryable<PaymentStatusModel>> GetPaymentStatusHistoryAsync(Expression<Func<PaymentStatusModel, bool>>? expression = null);
    }
}
