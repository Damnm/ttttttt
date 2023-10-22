using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.Models.Fees.PaidVehicleHistory;
using EPAY.ETC.Core.Models.Validation;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Payment
{
    public interface IPaymentRepository: IRepository<Core.Models.Payment.PaymentModel, Guid>
    {
        Task<List<PaidVehicleHistoryModel>?> GetPaidVehicleHistoryAsync(string? laneId = "1");
    }
}
