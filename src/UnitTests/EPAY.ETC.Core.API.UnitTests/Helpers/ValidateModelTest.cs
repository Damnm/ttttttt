using System.ComponentModel.DataAnnotations;

namespace EPAY.ETC.Core.API.UnitTests.Helpers
{
    public static class ValidateModelTest
    {
        public static List<ValidationResult> ValidateModel<T>(T model)
        {
            #nullable disable
            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(model, context, result, true);
            return result;
        }
    }
}
