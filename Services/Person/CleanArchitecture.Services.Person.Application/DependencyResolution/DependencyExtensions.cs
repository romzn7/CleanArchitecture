using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Services.Person.Application.DependencyResolution;

public static class DependencyExtensions
{
    public static IServiceCollection AddPersonApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyExtensions).Assembly;
        services.AddMediatR(assembly);
        services.AddAutoMapper(assembly);
        services.AddValidatorsFromAssembly(assembly);
        //services.TryAddSingleton<IMasterSettingsFileHelper, MasterSettingsFileHelper>();
        //services.TryAddScoped<ICachedPlatformLogoProvider, CachedPlatformLogoProvider>();

        return services;
    }
}