using EPAY.ETC.Core.API.Core.Interfaces.Repositories;
using EPAY.ETC.Core.API.Core.Models.Employees;

namespace EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.Employees
{
    public interface IEmployeeRepository : IGetAllRepository<EmployeeModel, string>
    {
    }
}
