using AutoMapper;
using EPAY.ETC.Core.API.Core.Interfaces.Services.TicketType;
using EPAY.ETC.Core.API.Core.Models.TicketType;
using EPAY.ETC.Core.API.Infrastructure.Persistence.Repositories.TicketType;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

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

       
        public async Task<ValidationResult<List<TicketTypeModel>?>> GetAllAsync(Expression<Func<TicketTypeModel, bool>>? expressison = null)
        {
            _logger.LogInformation($"Executing {nameof(GetAllAsync)} method...");
            try
            {
                var result = await _repository.GetAllAsync(expressison);
                if (result == null)
                {
                    return ValidationResult.Failed<List<TicketTypeModel>?>(null, new List<ValidationError>()
                    {
                        ValidationError.NotFound
                    });
                }

                return ValidationResult.Success(result.ToList() ?? null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to run {nameof(GetAllAsync)} method. Error: {ex.Message}");
                throw;
            }
        }
    }
}
