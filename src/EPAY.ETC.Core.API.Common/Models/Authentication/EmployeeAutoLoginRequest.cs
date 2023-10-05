using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Models.Authentication
{
    public class EmployeeAutoLoginRequest
    {
        public string EmployeeId { get; set; }
        public string ActionCode { get; set; }
    }
}
