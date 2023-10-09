namespace EPAY.ETC.Core.API.Infrastructure.Models.Configs
{
    public class JWTSettingsConfig
    {
        public string SecretKey { get; set; }
        public double ExpiresInDays { get; set; } = 0;
        public double ExpiresInHours { get; set; } = 0;
        public double ExpiresInMinutes { get; set; } = 0;
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
