using CleanArchitecture.Services.Shared.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Services.Shared.Application.Behaviours;

public class AsyncValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : MediatR.IRequest<TResponse>
{
    private readonly ILogger<AsyncValidatorBehavior<TRequest, TResponse>> _logger;
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public AsyncValidatorBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<AsyncValidatorBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var typeName = request.GetGenericTypeName();

        _logger.LogInformation("----- Validating command {CommandType}", typeName);

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v =>
                v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
                 .Where(r => r.Errors.Any())
                 .SelectMany(r => r.Errors)
                 .ToList();

        if (failures.Any())
        {
            _logger.LogWarning("Validation errors - {CommandType} - Request: {@Request} - Errors: {@ValidationErrors}", typeName, request, failures);

            throw new ValidationException($"Validation Errors for type {typeof(TRequest).FullName}", failures);
        }

        return await next();
    }

}
