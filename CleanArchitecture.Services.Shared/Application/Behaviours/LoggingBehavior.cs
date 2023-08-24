using CleanArchitecture.Services.Shared.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CleanArchitecture.Services.Shared.Application.Behaviours;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : MediatR.IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var timer = new Stopwatch();
        timer.Start();
        _logger.LogInformation("----- Handling command {CommandName} ({@Command})", request.GetGenericTypeName(), request);

        var response = await next();

        timer.Stop();
        _logger.LogInformation("----- Command {CommandName} handled {ElapsedMilliseconds}ms", request.GetGenericTypeName(), timer.ElapsedMilliseconds);

        return response;
    }

}