using EPAY.ETC.Core.API.Core.DtoModels.BaseDto;

namespace EPAY.ETC.Core.API.Core.Models.Payment
{
    public class PaymentDto : BaseDto<Guid>
    {
        public string? LaneInId { get; set; }
        public Guid? FeeId { get; set; }
        public string? LaneOutId { get; set; }
        public int Duration { get; set; }
        public string? RFID { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? PlateNumber { get; set; }
        public string? VehicleTypeId { get; set; }
        public Guid? CustomVehicleTypeId { get; set; }
        public double Amount { get; set; }
    }
}
