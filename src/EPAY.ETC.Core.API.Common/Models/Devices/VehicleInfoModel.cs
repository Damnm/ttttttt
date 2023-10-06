namespace EPAY.ETC.Core.API.Core.Models.Devices
{
    public class VehicleInfoModel
    {
        public string? Make { set; get; }
        public string? Model { set; get; }
        public string? PlateNumber { set; get; }
        public string? VehicleType { set; get; }
        public int? Seat { set; get; }
        public int? Weight { set; get; }
        public string? PlateNumberPhotoUrl { set; get; }
        public string? VehiclePhotoUrl { set; get; }
    }
}
