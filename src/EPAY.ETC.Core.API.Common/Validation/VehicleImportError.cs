using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Validation
{
    public class VehicleImportError
    {
        public string Filename { get; set; }
        public List<VehicleImportErrorDetail> ErrorDetails { get; set; }
    }
}
