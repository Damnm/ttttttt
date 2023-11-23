using EPAY.ETC.Core.API.Core.Models.TicketType;
using EPAY.ETC.Core.Models.Validation;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.TicketType
{
    public interface ITicketTypeService
    {
        Task<ValidationResult<List<TicketTypeModel>?>> GetAllAsync(Expression<Func<TicketTypeModel, bool>>? expressison = null);
    }
}
