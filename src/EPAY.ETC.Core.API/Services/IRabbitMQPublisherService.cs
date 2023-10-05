using EPAY.ETC.Core.Models.Enums;

namespace EPAY.ETC.Core.API.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRabbitMQPublisherService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="target"></param>
        void SendMessage(string message, PublisherTargetEnum target);
    }
}
