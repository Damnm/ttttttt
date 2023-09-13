using System.Diagnostics.CodeAnalysis;

namespace EPAY.ETC.Core.API.Core.Validation
{

    /// <summary>
    /// A standard response for service calls.
    /// </summary>
    /// <typeparam name="T">Return data type</typeparam>
    [ExcludeFromCodeCoverage]
    public class ValidationResult<T> : ValidationResult
    {
        public T Data { get; set; }

        public ValidationResult(T data)
        {
            Data = data;
        }

        public ValidationResult(T data, List<ValidationError> errors) : base(errors)
        {
            Data = data;
        }

        public ValidationResult(List<ValidationError> errors) : base(errors)
        {

        }
    }

    [ExcludeFromCodeCoverage]
    public class ValidationResult
    {
        public bool Succeeded => Errors == null || !Errors.Any();

        public List<ValidationError> Errors { get; set; }

        public ValidationResult(List<ValidationError> errors)
        {
            Errors = errors;
        }
        public ValidationResult() { }

        #region Helper Methods

        public static ValidationResult<T> Failed<T>(List<ValidationError> errors)
        {
            return new ValidationResult<T>(errors);
        }
        public static ValidationResult<T> Failed<T>(T? data, List<ValidationError> errors)
        {
            return new ValidationResult<T>(data!, errors);
        }
        public static ValidationResult<T> Success<T>(T data)
        {
            return new ValidationResult<T>(data);
        }

        public static ValidationResult<T> Failed<T>(ValidationError notFound)
        {
            throw new NotImplementedException();
        }

        public static ValidationResult<T> Failed<T>(string v)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
