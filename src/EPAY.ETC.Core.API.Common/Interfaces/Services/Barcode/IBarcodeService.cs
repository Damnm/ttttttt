using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.Barcode
{
    public interface IBarcodeService
    {
        public Task<ValidationResult<Models.Barcode.BarcodeModel>> AddAsync(BarcodeAddRequestModel input);
        public Task<ValidationResult<Models.Barcode.BarcodeModel>> GetByIdAsync(Guid id);
        public Task<ValidationResult<Models.Barcode.BarcodeModel>> UpdateAsync(Guid id, BarcodeUpdateRequestModel request);
        public Task<ValidationResult<Models.Barcode.BarcodeModel>> RemoveAsync(Guid id);
    }
}
