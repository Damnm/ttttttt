using EPAY.ETC.Core.API.Core.Interfaces.Services.Authentication;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace EPAY.ETC.Core.API.Infrastructure.Services.Authentication
{
    public class PasswordService : IPasswordService
    {
        private readonly ILogger<PasswordService> _logger;
        private readonly int _keyLength = 32;
        private readonly int _hashNumber = 981;
        private readonly int _byteNumber = 36;
        private readonly int _saltNumber = 16;
        private readonly int _hashPwdNumber = 20;

        public PasswordService(ILogger<PasswordService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ValidationResult<string> GetRandomSalt()
        {
            _logger.LogInformation($"Executing {nameof(GetRandomSalt)} method...");
            try
            {
                RandomNumberGenerator rngCryptoServiceProvider = RandomNumberGenerator.Create();
                byte[] randomBytes = new byte[_keyLength];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return ValidationResult.Success(Convert.ToBase64String(randomBytes));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(GetRandomSalt)} method. Error: {ex.Message}");
                throw;
            }
        }

        public ValidationResult<string> CreateHashedPassword(string plainPassword, string salt)
        {
            _logger.LogInformation($"Executing {nameof(CreateHashedPassword)} method...");
            try
            {
                ////Note: this salt needs to be stored in database along with hashed password so that we can use to check if user enters password correctly
                var saltByte = Convert.FromBase64String(salt);

                // The Rfc2989DeriveBytes class is going to be responsible for the hashing. 
                // 981 represents the number of iterations the algorithm is going to perform 
                // (It will keep hashing the previous hash this number of times. This is what makes it slower by design).
                var k1 = new Rfc2898DeriveBytes(plainPassword, saltByte, _hashNumber);

                // Get 20 bytes from k1
                var hash = k1.GetBytes(_hashPwdNumber);

                // Declare an of array of 36 bytes which will contain salt and hashed password
                var hashBytes = new byte[_byteNumber];

                //merge salt and hashed password
                Array.Copy(saltByte, 0, hashBytes, 0, _saltNumber);
                Array.Copy(hash, 0, hashBytes, _saltNumber, _hashPwdNumber);

                //Convert hashed password in byte array to string and store in database along with the salt.
                var hashedPassword = Convert.ToBase64String(hashBytes);

                //Implement the logic to save the saltString and hashedPassword here

                return ValidationResult.Success(hashedPassword);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(CreateHashedPassword)} method. Error: {ex.Message}");
                throw;
            }
        }

        public ValidationResult<bool> IsMatched(string plainPwd, string hashedPwd, string salt)
        {

            _logger.LogInformation($"Executing {nameof(IsMatched)} method...");
            try
            {
                //Convert salt string to byte array
                var saltByte = Convert.FromBase64String(salt);

                //Use the same way to hash password above. Note: the iteration of 10000 must be the same
                var k1 = new Rfc2898DeriveBytes(plainPwd, saltByte, _hashNumber);

                //Get 20 bytes from k1. This is the byte array which will be used to compare with hashedPassword stored in database
                var hash = k1.GetBytes(_hashPwdNumber);

                //Convert hashedPassword to byte array. Note: the first 16 bytes is salt and needs to be taken out before comparing
                var hashedBytes = Convert.FromBase64String(hashedPwd);

                for (var i = 0; i < hash.Length; i++)
                {
                    if (hash[i] != hashedBytes[_saltNumber + i])
                    {
                        return ValidationResult.Success(false);
                    }
                }
                return ValidationResult.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(IsMatched)} method. Error: {ex.Message}");
                throw;
            }
        }
    }
}
