using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAY.ETC.Core.API.UnitTests.Helpers
{
    public static class ValidateModelTest
    {
        public static List<ValidationResult> ValidateModel<T>(T model)
        {
            var context = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(model, context, result, true);
            return result;
        }
    }
}
