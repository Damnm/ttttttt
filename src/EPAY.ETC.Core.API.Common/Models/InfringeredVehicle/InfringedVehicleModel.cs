using EPAY.ETC.Core.API.Core.Entities;
using EPAY.ETC.Core.API.Core.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Models.InfringeredVehicle
{
    [ExcludeFromCodeCoverage]
    [Table("InfringedVehicle")]
    public class InfringedVehicleModel: BaseEntity<Guid>
    {
        [Key]
        [Column("InfringedVehicleId")]
        public new Guid Id { get; set; }
        [MaxLength(50)]
        public string? RFID { get; set; }
        [MaxLength(20)]
        public string? PlateNumber { get; set; }

        public InfringedTypeEnum InfringedType { get; set; }
        [MaxLength(20)]
        public string EmployeeId { get; set; }

        [MaxLength(100)]
        public string Desc { get; set; }
    }
}
