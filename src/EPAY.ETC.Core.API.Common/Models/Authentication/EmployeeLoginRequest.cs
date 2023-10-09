using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Models.Authentication
{
    public class EmployeeLoginRequest
    {
        public string EmployeeId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
