using EPAY.ETC.Core.API.Core.Interfaces.Services.ExternalServices;
using EPAY.ETC.Core.API.Infrastructure.Models.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace EPAY.ETC.Core.API.Infrastructure.Services.ExternalServices
{
    public class POSService : IPOSService
    {
        private readonly ILogger<POSService> _logger;
        private readonly HttpClient _httpClient;
        private readonly WalletUrlOptions _apiUrlConfigs;

        public POSService(ILogger<POSService> logger, HttpClient httpClient, IOptions<WalletUrlOptions> apiUrlConfigs)
        {
            if (apiUrlConfigs is null)
            {
                throw new ArgumentNullException(nameof(apiUrlConfigs));
            }

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiUrlConfigs = apiUrlConfigs.Value;
        }

        public async Task<string?> CancelPaymentAsync(string transId)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(CancelPaymentAsync)} method. Request data: {transId}");
                var response = await _httpClient.PostAsJsonAsync($"{_apiUrlConfigs.Void}", transId);

                var result = await response.Content.ReadAsStringAsync();

                _logger.LogWarning($"Response: {result}");

                response.EnsureSuccessStatusCode();

                return result;
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError($"An error occurred in {nameof(CancelPaymentAsync)} method. Message: {httpEx.Message}. Stack trace: {httpEx.StackTrace}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(CancelPaymentAsync)} method. Error: {ex.Message}");
                return null;
            }
        }
    }
}
