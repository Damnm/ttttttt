using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EPAY.ETC.Core.API.IntegrationTests.Helpers
{
    public static class MockJwtTokensHelper
    {
        public static string Issuer { get; } = "ACV Toll Admin";
        public static string Audience { get; } = "ACV Toll Admin";
        public static SecurityKey SecurityKey { get; }
        public static SigningCredentials SigningCredentials { get; }

        private static readonly JwtSecurityTokenHandler s_tokenHandler = new JwtSecurityTokenHandler();

        static MockJwtTokensHelper()
        {
            var key = Encoding.ASCII.GetBytes("^7tYysfQ8av@wJgAU8*@&AX7hry4xcxe");
            SecurityKey = new SymmetricSecurityKey(key);
            SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);
        }

        public static string GenerateJwtToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "Test user")
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = SigningCredentials,
                Issuer = Issuer,
                Audience = Audience,
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return s_tokenHandler.WriteToken(token);
        }
    }
}
