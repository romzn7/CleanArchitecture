using CleanArchitecture.Services.Shared.Models.Configurations;
using Microsoft.AspNetCore.Builder;

namespace CleanArchitecture.Services.Shared.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseApplicationCORS(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseCors(WebApiSettings.CORSPolicy);

        return applicationBuilder;
    }
}
