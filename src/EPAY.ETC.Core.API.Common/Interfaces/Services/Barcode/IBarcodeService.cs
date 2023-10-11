using EPAY.ETC.Core.API.Core.Models.Barcode;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using System.Linq.Expressions;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.Barcode
{
    public interface IBarcodeService
    {
        public Task<ValidationResult<BarcodeModel>> AddAsync(BarcodeAddOrUpdateRequestModel input);
        public Task<ValidationResult<BarcodeModel>> GetByIdAsync(Guid id);
        public Task<ValidationResult<BarcodeModel>> UpdateAsync(Guid id, BarcodeAddOrUpdateRequestModel request);
        public Task<ValidationResult<BarcodeModel>> RemoveAsync(Guid id);
        public Task<ValidationResult<List<BarcodeModel>>> GetListAsync(Expression<Func<BarcodeModel, bool>>? expression);
    }
}
