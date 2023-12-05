using EPAY.ETC.Core.API.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPAY.ETC.Core.API.Core.Models.Employees
{
    [Table("Employee")]
    public class EmployeeModel : BaseEntity<string>
    {
        [MaxLength(2)]
        public string StationId { get; set; }
        [MaxLength(6)]
        public string? TeamId { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(50)]
        public string? NickName { get; set; }
        [MaxLength(50)]
        public string? Email { get; set; }
        [MaxLength(50)]
        public string? UserName { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }
        [MaxLength(255)]
        public string Salt { get; set; }
        public DateTime? Birthday { get; set; }
        public Int16? Gender { get; set; }
        [MaxLength(200)]
        public string? Address { get; set; }
        [MaxLength(50)]
        public string? Mobile { get; set; }
        public string FullName { get { return $"{FirstName} {LastName}"; } }
    }
}
