using EPAY.ETC.Core.API.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Models.ErrorResponse
{
    [ExcludeFromCodeCoverage]
    [Table("ErrorResponse")]
    public class ErrorResponseModel : BaseEntity<Guid>
    {
        [MaxLength(10)]
        public string? Source { get; set; }
        [MaxLength(10)]
        public string? Function { get; set; }
        [MaxLength(30)]
        public string? Code { get; set; }
        [MaxLength(30)]
        public string? Status { get; set; }
        [MaxLength(30)]
        public string? ErrorCode { get; set; }
        [MaxLength(30)]
        public string? EpayCode { get; set; }
        [MaxLength(30)]
        public string? EpayMessage { get; set; }
    }
}
