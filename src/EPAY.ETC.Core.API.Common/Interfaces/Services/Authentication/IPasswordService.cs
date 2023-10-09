using EPAY.ETC.Core.Models.Validation;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.Authentication
{
    public interface IPasswordService
    {
        ValidationResult<string> GetRandomSalt();
        ValidationResult<string> CreateHashedPassword(string plainPassword, string salt);
        ValidationResult<bool> IsMatched(string plainPwd, string hashedPwd, string salt);
    }
}
