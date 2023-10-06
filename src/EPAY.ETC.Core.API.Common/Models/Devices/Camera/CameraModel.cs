namespace EPAY.ETC.Core.API.Core.Models.Devices.Camera
{
    public class CameraModel
    {
        public string TagId { get; set; }
        public long Epoch { get; set; }
        public DeviceModel? CameraDeviceInfo { get; set; }
        public VehicleInfoModel? VehicleInfo { get; set; }
    }
}
