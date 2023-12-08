namespace EPAY.ETC.Core.API.Core.Interfaces.Services.ExternalServices
{
    public interface IPOSService
    {
        Task<string?> CancelPaymentAsync(string transId);
    }
}
