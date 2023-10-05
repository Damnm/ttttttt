using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.Models.Enums;

namespace EPAY.ETC.Core.API.Core.Models.Authentication
{
    public class AuthenticatedEmployeeModel: BaseEntity<Guid>
    {

        public string? EmployeeId { get; set; }

        public string? Username { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? JwtToken { get; set; }

        public LogonStatusEnum? Action { get; set; }
    }
}
