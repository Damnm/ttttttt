using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.ErrorResponse;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.ErrorResponse
{
    public interface IErrorResponseRepository : IGetAllRepository<ErrorResponseModel, Guid>
    {
    }
}
