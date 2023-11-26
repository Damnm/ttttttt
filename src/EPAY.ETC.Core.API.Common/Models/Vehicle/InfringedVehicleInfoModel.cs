namespace EPAY.ETC.Core.API.Core.Models.Vehicle
{
    public class InfringedVehicleInfoModel
    {
        public Guid ?InfringedVehicleId { get; set; }
        public string? InfringedPlateNumber { get; set; }
        public string? InfringedRFID { get; set; }
    }
}
