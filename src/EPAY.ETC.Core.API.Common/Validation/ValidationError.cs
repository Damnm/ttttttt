using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Validation
{
    /// <summary>
    /// All errors contained in ValidationResult objects must return an error of this type
    /// Error codes allow the caller to easily identify the received error and take action.
    /// Error messages allow the caller to easily show error messages to the end user.
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class ValidationError
    {
        /// <summary>
        /// CTOR
        /// </summary>
        public ValidationError(string message, int code)
        {
            Message = message;
            Code = code;
        }

        public ValidationError() { }

        /// <summary>
        /// Human readable error message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Machine readable error code
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// Default validation error. Use this for invalid parameters in controller actions and service methods.
        /// </summary>
        public static ValidationError ModelStateError(string validationError)
        {
            return new ValidationError(validationError, 998);
        }

        public static ValidationError CustomMessage(string errorMessage)
        {
            return new ValidationError(errorMessage, 997);
        }
        public static ValidationError BadRequest => new ValidationError("Bad request.", 400);
        public static ValidationError NotFound => new ValidationError("Not found", 404);
        public static ValidationError InternalServerError => new ValidationError("Internal server error.", 500);
        public static ValidationError Conflict => new ValidationError("Conflict.", 409);
    }

}
