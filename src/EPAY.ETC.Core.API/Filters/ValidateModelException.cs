using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EPAY.ETC.Core.API.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class ValidateModelException : ModelStateDictionary
    {
        /// <summary>
        /// 
        /// </summary>
        public ValidateModelException()
        {
            Errors = new Dictionary<string, List<string>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelState"></param>
        public ValidateModelException(ModelStateDictionary modelState)
            : this()
        {
            foreach (string key in modelState.Keys)
            {
                var property = modelState.GetValueOrDefault(key);

                List<string> errors = property?.Errors.Select(error => error.ErrorMessage).ToList() ?? new List<string>();

                Errors.Add(key, errors);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, List<string>> Errors { get; }
    }
}
