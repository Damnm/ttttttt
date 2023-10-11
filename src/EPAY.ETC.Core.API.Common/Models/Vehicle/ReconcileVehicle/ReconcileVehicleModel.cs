namespace EPAY.ETC.Core.API.Core.Models.Vehicle.ReconcileVehicle
{
    public class ReconcileVehicleModel
    {
        public string? PlateNumber { get; set; }    
        public string? RFID { get; set; }
        public string? VehicleType { get; set; }
        public LaneInModel? In { get; set; }
        public LaneOutModel? Out { get; set; }
    }
}
