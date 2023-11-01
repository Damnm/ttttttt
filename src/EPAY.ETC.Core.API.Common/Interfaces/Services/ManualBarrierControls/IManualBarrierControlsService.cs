using EPAY.ETC.Core.API.Core.Models.ManualBarrierControl;
using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.ManualBarrierControls
{
    public interface IManualBarrierControlsService
    {
        public Task<ValidationResult<ManualBarrierControlModel>> AddAsync(ManualBarrierControlAddOrUpdateRequestModel input);
        public Task<ValidationResult<ManualBarrierControlModel>> GetByIdAsync(Guid id);
        public Task<ValidationResult<ManualBarrierControlModel>> UpdateAsync(Guid id, ManualBarrierControlAddOrUpdateRequestModel request);
        public Task<ValidationResult<ManualBarrierControlModel?>> RemoveAsync(Guid id);
    }
}
