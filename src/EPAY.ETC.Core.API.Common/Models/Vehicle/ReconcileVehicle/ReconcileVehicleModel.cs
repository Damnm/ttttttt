using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Models.Vehicle.ReconcileVehicle
{
    public class ReconcileVehicleModel
    {
        public string? PlateNumber { get; set; }    
        public string? RFID { get; set; }
        public string? VehicleType { get; set; }
        public LandModel LandIn { get; set; }
        public LandModel LandOut { get; set; }
    }
}
