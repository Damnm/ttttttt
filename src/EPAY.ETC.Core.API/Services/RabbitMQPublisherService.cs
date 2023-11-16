using AutoMapper;
using EPAY.ETC.Core.API.Core.Exceptions;
using EPAY.ETC.Core.API.Models.Configs;
using EPAY.ETC.Core.Models.Constants;
using EPAY.ETC.Core.Models.Enums;
using EPAY.ETC.Core.Publisher.Common.Options;
using EPAY.ETC.Core.Publisher.Interface;
using EPAY.ETC.Core.RabbitMQ.Common.Events;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace EPAY.ETC.Core.API.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class RabbitMQPublisherService : IRabbitMQPublisherService
    {
        private readonly ILogger<RabbitMQPublisherService> _logger;
        private readonly IPublisherService _publisherService;
        private readonly IMapper _mapper;
        private readonly List<PublisherConfigurationOption> _publisherOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="publisherService"></param>
        /// <param name="publisherOptions"></param>
        /// <param name="mapper"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RabbitMQPublisherService(ILogger<RabbitMQPublisherService> logger, IPublisherService publisherService, IOptions<List<PublisherConfigurationOption>> publisherOptions, IMapper mapper)
        {
            if (publisherOptions is null)
            {
                throw new ArgumentNullException(nameof(publisherOptions));
            }

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _publisherService = publisherService ?? throw new ArgumentNullException(nameof(publisherService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _publisherOptions = publisherOptions.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="target"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SendMessage(string message, PublisherTargetEnum target)
        {
            try
            {
                _logger.LogInformation($"Executing {nameof(SendMessage)} with laneId={Environment.GetEnvironmentVariable(CoreConstant.ENVIRONMENT_LANE_OUT)}...");

                var publisherOption = _publisherOptions.FirstOrDefault(x => x.PublisherTarget == target);

                if (publisherOption != null && publisherOption.BindArguments.ContainsKey(CoreConstant.RABBIT_HEADER_PROP_LANEID))
                    publisherOption.BindArguments[CoreConstant.RABBIT_HEADER_PROP_LANEID] = Environment.GetEnvironmentVariable(CoreConstant.ENVIRONMENT_LANE_OUT) ?? publisherOption.BindArguments[CoreConstant.RABBIT_HEADER_PROP_LANEID];

                _logger.LogInformation($"Publiser option: {JsonSerializer.Serialize(publisherOption)}");

                RabbitMessageOutbound resultMessage = new RabbitMessageOutbound()
                {
                    Message = message
                };

                _publisherService.SendMessage(resultMessage, _mapper.Map<PublisherOptions>(publisherOption));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when calling {nameof(SendMessage)} method: {ex.Message}. InnerException : {ApiExceptionMessages.ExceptionMessages(ex)}. Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
