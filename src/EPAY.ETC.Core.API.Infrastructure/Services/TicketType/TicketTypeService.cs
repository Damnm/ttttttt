using AutoMapper;
using EPAY.ETC.Core.API.Core.Interfaces.Services.TicketType;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TicketType;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.Extensions.Logging;

namespace EPAY.ETC.Core.API.Infrastructure.Services.TicketType
{
    public class TicketTypeService : ITicketTypeService
    {

        #region Variables
        private readonly ILogger<TicketTypeService> _logger;
        private readonly ITicketTypeRepository _repository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public TicketTypeService(ILogger<TicketTypeService> logger, ITicketTypeRepository ticketTypeRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = ticketTypeRepository ?? throw new ArgumentNullException(nameof(ticketTypeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        #endregion

       
        public async Task<ValidationResult<Guid?>> GetByCodeAsync(string code)
        {
            _logger.LogInformation($"Executing {nameof(GetByCodeAsync)} method...");
            try
            {
                var result = await _repository.GetByCodeAsync(code);
                if (result == null)
                {
                    return ValidationResult.Failed<Guid?>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }

                return ValidationResult.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(GetByCodeAsync)} method. Error: {ex.Message}");
                throw;
            }
        }
    }
}
