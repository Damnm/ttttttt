using System.Text.Json.Serialization;

namespace EPAY.ETC.Core.API.Core.Validation
{
    public class VehicleImportErrorDetail
    {
        public int RowNumber { get; set; }
        public string Message { get; set; }
        [JsonIgnore]
        public string FieldName { get; set; }
    }
}
