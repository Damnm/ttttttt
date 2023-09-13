using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.TimeBlockFees;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TimeBlockFees
{
    public interface ITimeBlockFeeFormulaRepository : IGetAllRepository<TimeBlockFeeFormulaModel, Guid>
    {
    }
}
