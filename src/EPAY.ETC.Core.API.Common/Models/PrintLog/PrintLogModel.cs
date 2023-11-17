using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EPAY.ETC.Core.API.Core.Models.PrintLog
{
    public class PrintLogModel: BaseEntity<Guid>
    {
        [Key]
        [Column("PrintLogId")]
        public new Guid Id { get; set; }

        [MaxLength(10)]
        public string? LaneOutId { get; set; }
        [MaxLength(50)]
        public string? EmployeeId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PrintLogEnum? PrintType { get; set; }
        [MaxLength(50)]
        public string? RFID { get; set; }

        [MaxLength(20)]
        public string? PlateNumber { get; set; }
        public string? DataJson { get; set; }
    }
}
