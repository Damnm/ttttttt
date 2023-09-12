using EPAY.ETC.Core.API.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;


namespace EPAY.ETC.Core.API.IntegrationTests.Common
{
    public class IntegrationTestBase : IDisposable
    {
        protected WebApplicationFactory<Program> WebApplicationFactory;
        protected HttpClient HttpClient;
        protected string JWTToken;
        public IntegrationTestBase()
        {
            WebApplicationFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment("IntegrationTests");
                    builder.ConfigureServices(services =>
                    {
                        var configBuilder = new ConfigurationBuilder()
                                            .AddJsonFile("appsettings.IntegrationTests.json")
                                            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
                                            .Build();

                        builder.UseConfiguration(configBuilder);
                    });

                    builder.ConfigureTestServices(services =>
                    {
                        services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                        {
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,
                                ValidIssuer = MockJwtTokensHelper.Issuer,
                                ValidAudience = MockJwtTokensHelper.Audience,
                                IssuerSigningKey = MockJwtTokensHelper.SecurityKey,
                            };
                        });
                    });
                });
            HttpClient = WebApplicationFactory.CreateClient();
            JWTToken = MockJwtTokensHelper.GenerateJwtToken();

        }

        public void Dispose()
        {
            // Do not change this code
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposedValue;

        private void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {
                HttpClient.Dispose();
                WebApplicationFactory.Dispose();
            }

            _disposedValue = true;
        }
    }
}
