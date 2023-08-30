using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
                                            .Build();

                        builder.UseConfiguration(configBuilder);
                    });
                });
            HttpClient = WebApplicationFactory.CreateClient();
           
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
