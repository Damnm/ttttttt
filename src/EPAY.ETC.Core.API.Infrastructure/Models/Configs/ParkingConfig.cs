using EPAY.ETC.Core.API.Core.Models.Enum;
using EPAY.ETC.Core.Models.Enums;
using System.Text.Json.Serialization;

namespace EPAY.ETC.Core.API.Infrastructure.Models.Configs
{
    public class ParkingConfig
    {
        public string ParkingLocationId { get; set; } = string.Empty;
        public int DeltaTInSeconds { get; set; } = 300;
        public YesNoEnum ParkingFeesApplied { get; set; } = YesNoEnum.No;
    }

    public class ParkingLaneConfig
    {
        public string? ParkingLocationId { get; set; }
        public string LaneId { get; set; } = string.Empty;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaidStatusEnum ParkingPaidStatus { get; set; } = PaidStatusEnum.Unpaid;
    }
}
