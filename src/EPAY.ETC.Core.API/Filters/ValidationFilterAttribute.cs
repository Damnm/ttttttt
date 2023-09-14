using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using EPAY.ETC.Core.API.Core.Validation;

namespace EPAY.ETC.Core.API.Filters
{
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var exception = new ValidateModelException(context.ModelState);
                List<ValidationError> validationErrors = new List<ValidationError>();
                validationErrors.Add(ValidationError.BadRequest);
                context.Result = new BadRequestObjectResult(ValidationResult.Failed(exception.Errors, validationErrors));
                context.ExceptionHandled = true;
            }
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var exception = new ValidateModelException(context.ModelState);
                List<ValidationError> validationErrors = new List<ValidationError>();
                validationErrors.Add(ValidationError.BadRequest);
                context.Result = new BadRequestObjectResult(ValidationResult.Failed(exception.Errors, validationErrors));
            }
        }
    }
}
