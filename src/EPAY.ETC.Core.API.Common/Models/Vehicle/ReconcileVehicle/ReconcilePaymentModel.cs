using EPAY.ETC.Core.Models.Enums;
using System.Text.Json.Serialization;

namespace EPAY.ETC.Core.API.Core.Models.Vehicle.ReconcileVehicle
{
    public class ReconcilePaymentModel
    {
        public Guid? PaymentId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethodEnum PaymentType { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatusEnum PaymentStatus { get; set; }
    }
}
