using EPAY.ETC.Core.Models.Fees;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.UIActions
{
    public interface IUIActionLoadParkingDataService
    {
        ParkingModel? LoadParkingData(string? rfid, string? plateNumber);
    }
}
