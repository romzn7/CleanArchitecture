using CleanArchitecture.Services.Shared.Helpers;
using CleanArchitecture.Services.Shared.Models.Attributes;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace CleanArchitecture.Services.Shared.Application.Behaviours;

public class XssSanitizeBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : MediatR.IRequest<TResponse>
{
    private readonly ILogger<XssSanitizeBehavior<TRequest, TResponse>> _logger;
    private readonly IHtmlInputSanitizer _htmlInputSanitizer;
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public XssSanitizeBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<XssSanitizeBehavior<TRequest, TResponse>> logger, IHtmlInputSanitizer htmlInputSanitizer)
    {
        _validators = validators;
        _logger = logger;
        this._htmlInputSanitizer = htmlInputSanitizer;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request != null)
        {
            // Use reflection to iterate through the properties of the object
            var properties = GetProperties(request.GetType());
            TraverseProperties(request, properties);
        }

        // Call the next pipeline behavior or handler
        var response = await next();

        return response;
    }

    #region Helper

    private IReadOnlyList<(string name, Type type)> GetProperties(Type type)
    {
        var properties = new List<(string name, Type type)>();

        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (property.PropertyType == typeof(string) && property.GetCustomAttribute<StripHtmlAttribute>() != null)
            {
                properties.Add((property.Name, property.PropertyType));
            }
            else if (property.PropertyType.IsGenericType &&
                typeof(System.Collections.IEnumerable).IsAssignableFrom(property.PropertyType))
            {
                var subProperties = GetProperties(property.PropertyType.GenericTypeArguments[0])
                    .Select(p => (property.Name + "." + p.name, p.type));
                properties.AddRange(subProperties);
            }
            else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
            {
                var subProperties = GetProperties(property.PropertyType)
                    .Select(p => (property.Name + "." + p.name, p.type));
                properties.AddRange(subProperties);
            }
        }

        return properties;
    }

    private void TraverseProperties(object obj, IReadOnlyList<(string name, Type type)> propertyInfos)
    {
        foreach (var propertyInfo in propertyInfos)
        {
            var value = GetPropertyValue(obj, propertyInfo.name);
            if (value != null && propertyInfo.type == typeof(string))
            {
                var sanitizedValue = _htmlInputSanitizer.RemoveHtmlElemets(value?.ToString());
                sanitizedValue = System.Web.HttpUtility.HtmlDecode(sanitizedValue);
                SetPropertyValue(obj, propertyInfo.name, sanitizedValue);
            }
            else if (value != null)
            {
                TraverseProperties(value, GetProperties(propertyInfo.type));
            }
        }
    }

    public static object GetPropertyValue(object obj, string propertyName)
    {
        var propertyNames = propertyName.Split('.');
        var value = obj;

        foreach (var name in propertyNames)
        {
            var property = value?.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                return null;
            }

            try
            {
                value = property.GetValue(value);
            }
            catch (Exception ex)
            {

            }
        }

        return value;
    }

    public static void SetPropertyValue(object obj, string propertyName, object value)
    {
        if (obj is null)
            return;

        var propertyNames = propertyName.Split('.');

        // Traverse the object graph to locate the target object
        for (var i = 0; i < propertyNames.Length - 1; i++)
        {
            var property = obj!.GetType().GetProperty(propertyNames[i]);
            if (property == null)
            {
                return;
            }

            obj = property.GetValue(obj)!;
        }

        // Set the target property value
        var targetProperty = obj.GetType().GetProperty(propertyNames.Last());
        if (targetProperty != null && targetProperty.PropertyType == typeof(string))
        {
            targetProperty.SetValue(obj, value);
        }
    }

    #endregion
}