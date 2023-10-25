using FluentValidation.Results;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AutheticationServer.Extensions
{
    public static class ValidationErrorExtensions
    {
        public static string GetErrors(this List<ValidationFailure> errors)
        {
            var errorMessages = "";
            errors.ForEach(err =>  errorMessages += err.ErrorMessage + "");
            return errorMessages;
        }
    }
}
