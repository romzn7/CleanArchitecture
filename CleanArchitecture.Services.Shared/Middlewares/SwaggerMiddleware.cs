using CleanArchitecture.Services.Shared.Extensions;
using CleanArchitecture.Services.Shared.Models.Configurations;
using CleanArchitecture.Services.Shared.Models.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CleanArchitecture.Services.Shared.Middlewares;

public static class SwaggerMiddleware
{
    public static IApplicationBuilder UseCleanArchitectureSwagger(this IApplicationBuilder app, IConfiguration configuration, IHostEnvironment env, Action<List<SwaggerEndpointDefinition>> swaggerEndpointsConfigurations = null)
    {
        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger(options =>
        {
            options.PreSerializeFilters.Add((swagger, httpReq) =>
            {
                swagger.Servers = new List<OpenApiServer> {
                    new OpenApiServer { Url = $"https://{httpReq.Host.Value}" }
                };
            });

            options.RouteTemplate = "swagger/{documentName}/swagger.json";
            options.SerializeAsV2 = true;
        });

        //var ssoSettings = configuration.GetSection(nameof(SSOSettings)).Get<SSOSettings>();

        //if (ssoSettings == null)
        //    throw new InvalidOperationException($"Missing configuration {nameof(SSOSettings)}");

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        // specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c =>
        {
            //application specific endpoints
            if (swaggerEndpointsConfigurations != null)
            {
                var endpoints = new List<SwaggerEndpointDefinition>();
                swaggerEndpointsConfigurations.Invoke(endpoints);
                if (endpoints?.Any(e => string.IsNullOrEmpty(e.Name) || string.IsNullOrEmpty(e.Url)) ?? true)
                    throw new InvalidOperationException($"Invalid endpoint configuration found");

                foreach (var endpoint in endpoints)
                    c.SwaggerEndpoint(endpoint.Url, endpoint.Name);
            }

            //Dev credentials
            c.OAuthClientId("imagegalleryclient");

            //Enable for dev for now since we cannot add any claims to auth
            if (env.IsDevelopmentEnvironment())
                c.OAuthClientSecret("apisecret");

            c.OAuthUsePkce();

            //Prevents swagger from using cookie auth
            c.Interceptors.RequestInterceptorFunction = "function (req) {  req.credentials = 'omit'; return req;}";
        });

        return app;
    }
}

