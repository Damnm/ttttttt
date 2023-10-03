using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Publisher.Common.Options;

namespace EPAY.ETC.Core.API.Models.Configs
{
    /// <summary>
    /// Publisher option can use when publish message
    /// </summary>
    public class PublisherConfigurationOption : PublisherOptions
    {
        /// <summary>
        /// Target option of publisher
        /// </summary>
        public PublisherTargetEnum PublisherTarget { get; set; }
    }
}
