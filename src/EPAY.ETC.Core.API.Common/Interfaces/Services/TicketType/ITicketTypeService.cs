using EPAY.ETC.Core.Models.Validation;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.TicketType
{
    public interface ITicketTypeService
    {
        Task<ValidationResult<Guid?>> GetByCodeAsync(string code);
    }
}
