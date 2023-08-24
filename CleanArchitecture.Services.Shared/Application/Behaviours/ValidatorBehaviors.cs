using CleanArchitecture.Services.Shared.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Services.Shared.Application.Behaviours;

public class ValidatorBehaviors<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : MediatR.IRequest<TResponse>
{
    private readonly ILogger<ValidatorBehaviors<TRequest, TResponse>> _logger;
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidatorBehaviors(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidatorBehaviors<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var typeName = request.GetGenericTypeName();

        _logger.LogInformation("----- Validating command {CommandType}", typeName);

        var failures = _validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (failures.Any())
        {
            _logger.LogWarning("Validation errors - {CommandType} - Request: {@Request} - Errors: {@ValidationErrors}", typeName, request, failures);

            throw new ValidationException($"Validation Errors for type {typeof(TRequest).FullName}", failures);
        }

        return await next();
    }
}
