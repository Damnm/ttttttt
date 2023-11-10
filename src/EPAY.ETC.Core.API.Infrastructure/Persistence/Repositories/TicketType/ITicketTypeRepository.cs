using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.TicketType;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ETCCheckouts
{
    public interface ITicketTypeRepository : IGetAllRepository<TicketTypeModel, Guid>
    {
    }
}
