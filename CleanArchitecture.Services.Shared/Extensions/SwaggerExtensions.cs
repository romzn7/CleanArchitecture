using CleanArchitecture.Services.Shared.Models.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CleanArchitecture.Services.Shared.Extensions;

public static class SwaggerExtensions
{
    public static SwaggerGenOptions AddApplicationSwaggerGen(this SwaggerGenOptions options, IConfiguration configuration)
    {
        var ssoSettings = configuration.GetSection(nameof(SSOSettings)).Get<SSOSettings>();

        // Define the OAuth2.0 scheme that's in use (i.e. Implicit Flow)
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Scheme = "Bearer",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri($"https://localhost:5001/connect/authorize", UriKind.Absolute),
                    TokenUrl = new Uri($"https://localhost:5001/connect/token", UriKind.Absolute),
                    Scopes = {
                        {"openid", "openid" },
                            { "imagegalleryapi.fullaccess", "full access" }
                                }
                }
            }
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new[] { "imagegalleryapi.fullaccess", "openid" }
            }
        });

        return options;
    }

    public static SwaggerGenOptions FinalizeApplicationSwaggerDocs(this SwaggerGenOptions options, IConfiguration configuration)
    {
        var skynetSettings = configuration.GetSection(nameof(SkynetSettings)).Get<SkynetSettings>();

        foreach (var swaggerDocs in options.SwaggerGeneratorOptions.SwaggerDocs.Select(c => c.Value))
        {
            //swaggerDocs.TermsOfService = new Uri($"{skynetSettings.UIRootUrl}terms");
            //swaggerDocs.Contact = new OpenApiContact
            //{
            //    Url = new Uri($"{skynetSettings.UIRootUrl}contact"),
            //    Name = "SKYNET",
            //    Email = skynetSettings.EmailContact
            //};
            //swaggerDocs.License = new OpenApiLicense
            //{
            //    Name = "Use under LICX",
            //    Url = new Uri($"{skynetSettings.UIRootUrl}license")
            //};
        }

        return options;
    }
}