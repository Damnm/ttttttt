using EPAY.ETC.Core.Models.Validation;
using System.Linq.Expressions;
using CoreModel = EPAY.ETC.Core.Models.Fees;
using EPAY.ETC.Core.API.Core.Models.Fees;
using EPAY.ETC.Core.Models.Fees;
using FeeModel = EPAY.ETC.Core.API.Core.Models.Fees.FeeModel;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.Fees
{
    public interface IFeeService
    {
        public Task<ValidationResult<FeeModel>> AddAsync(CoreModel.FeeModel input);
        public Task<ValidationResult<FeeModel?>> GetByIdAsync(Guid id);
        public Task<ValidationResult<IEnumerable<FeeModel>>> GetAllAsync(Expression<Func<FeeModel, bool>>? expressison = null);
        public Task<ValidationResult<FeeModel>> UpdateAsync(Guid id, CoreModel.FeeModel request);
        public Task<ValidationResult<FeeModel?>> RemoveAsync(Guid id);
        public Task<ValidationResult<CoreModel.FeeModel?>> GetByObjectIdAsync(string objectId);
        public Task<ValidationResult<List<LaneInVehicleModel>>> FindVehicleAsync(string numberPlate = null, string rfid = null);
    }
}
