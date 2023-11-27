namespace EPAY.ETC.Core.API.Core.Models.Vehicle
{
    public class InfringedVehicleInfoModel
    {
        public Guid ?InfringedVehicleId { get; set; }
        public string? PlateNumber { get; set; }
        public string? RFID { get; set; }
    }
}
