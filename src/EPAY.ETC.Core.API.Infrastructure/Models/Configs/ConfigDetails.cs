namespace EPAY.ETC.Core.API.Infrastructure.Models.Configs
{
    public class ConfigDetails
    {
        public AppConfig AppConfig { get; set; }
        public List<ParkingConfig>? ParkingConfigs { get; set; }
        public List<ParkingLaneConfig>? ParkingLaneConfigs { get; set; }
    }
}
