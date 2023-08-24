using FluentValidation;
using FluentValidation.Results;

namespace CleanArchitecture.Services.Shared.Extensions;

public static class FluentValidationExtensions
{
    public sealed class ValidationExceptionExtended : ValidationException
    {
        public ValidationExceptionExtended(ValidationFailure validationFailure)
            : base(new ValidationFailure[] { validationFailure })
        {
        }
    }
}