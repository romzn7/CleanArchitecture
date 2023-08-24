using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace CleanArchitecture.Services.Shared.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ValidationException vex)
        {
            await HandleValidationExceptionAsync(httpContext, vex);
        }
        catch (ArgumentNullException aex)
        {
            await HandleArgumentExceptionAsync(httpContext, aex);
        }
        catch (ArgumentException anex)
        {
            await HandleArgumentExceptionAsync(httpContext, anex);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(httpContext, exception);
        }
    }

    private Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        try
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                Message = exception.Message,
                Errors = exception.Errors.DistinctBy(p => p.ErrorMessage).Select(p => p.ErrorMessage)
            }));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Cannot log error {0}", exception);
        }

        return context.Response.WriteAsync(JsonSerializer.Serialize(GetException(exception)));
    }

    private Task HandleArgumentExceptionAsync(HttpContext context, ArgumentException exception)
    {
        try
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                Message = exception.Message,
                Errors = exception.ParamName
            }));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Cannot log error {0}", exception);
        }

        return context.Response.WriteAsync(JsonSerializer.Serialize(GetException(exception)));
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        try
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(JsonSerializer.Serialize(GetException(exception)));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Cannot log error {0}", exception);
        }

        return context.Response.WriteAsync(JsonSerializer.Serialize(GetException(exception)));
    }

    private Object GetException(Exception exception)
    {
        var InnerException = exception.InnerException == null ? null : GetException(exception.InnerException);
        var result = new
        {
            exception.Message
        };
        return result;
    }
}

