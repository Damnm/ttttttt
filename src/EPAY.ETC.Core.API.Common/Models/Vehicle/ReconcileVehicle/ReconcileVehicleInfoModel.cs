namespace EPAY.ETC.Core.API.Core.Models.Vehicle.ReconcileVehicle
{
    public class ReconcileVehicleInfoModel
    {
        public Guid? ObjectId { get; set; }
        public string? EmployeeId { get; set; }
        
        public VehicleModel? Vehicle { get; set; }  
        public PaymentModel? Payment { get; set; }
    }
}
