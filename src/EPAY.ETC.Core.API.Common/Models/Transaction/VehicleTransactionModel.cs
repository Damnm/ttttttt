using EPAY.ETC.Core.API.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Models.Transaction
{
    public class VehicleTransactionModel : BaseEntity<Guid>
    {
        
        public Guid LaneInId { get; set; }
        public DateTime LaneInDate { get; set; }
        public Guid LaneOutId { get; set; }
        public DateTime LaneOutDate { get; set; }
        public int Duration { get; set; }
        public string? PlateNumber { get; set; }
        public string? RFID { get; set; }
        public string? LaneInPlateNumberPhotoURL { get; set; }
        public string? LaneInVehiclePhotoURL { get; set; }
        public string? LaneOutPlateNumberPhotoURL { get; set; }
        public string? LaneOutVehiclePhotoURL { get; set; }
        public string? PaymentMethod { get; set; }
        public double Amount { get; set; }
        public Guid TicketId { get; set; }
        public Guid ExternalEmployeeId { get; set; }
        public Guid ShiftId { get; set; }
        public Guid VehicleId { get; set; }
    }
}
