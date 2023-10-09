namespace EPAY.ETC.Core.API.Core.Models.Employees
{
    public class EmployeeModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string FullName { get { return $"{FirstName} {LastName}"; } }
    }
}
