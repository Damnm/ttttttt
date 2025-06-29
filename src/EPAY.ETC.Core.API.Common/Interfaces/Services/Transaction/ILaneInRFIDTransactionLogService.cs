﻿using EPAY.ETC.Core.Models.Request;
using EPAY.ETC.Core.Models.Validation;

namespace EPAY.ETC.Core.API.Core.Interfaces.Services.Transaction
{
    public interface ILaneInRFIDTransactionLogService
    {
        Task<ValidationResult<bool>> AddOrUpdateAsync(Guid? id, LaneInRFIDTransactionLogRequestModel input);
    }
}
