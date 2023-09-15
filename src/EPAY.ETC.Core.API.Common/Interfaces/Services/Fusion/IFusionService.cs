using EPAY.ETC.Core.API.Core.Models.Fusion;
using EPAY.ETC.Core.Models.Validation;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.Fusion
{
    public interface IFusionService
    {
        public Task<ValidationResult<FusionModel>> AddAsync(FusionAddRequestModel input);
        public Task<ValidationResult<FusionModel>> GetByIdAsync(Guid id);
        public Task<ValidationResult<FusionModel>> UpdateAsync(Guid id, FusionUpdateRequestModel request);
        public Task<ValidationResult<FusionModel>> RemoveAsync(Guid id);
    }
}
