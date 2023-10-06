namespace EPAY.ETC.Core.API.Core.Models.Devices.RFID
{
    public class RFIDModel
    {
        public string TagId { get; set; }
        public long Epoch { get; set; }
        public DeviceModel? RFIDDeviceInfo { get; set; }
        public VehicleInfoModel? VehicleInfo { get; set; }
    }
}
