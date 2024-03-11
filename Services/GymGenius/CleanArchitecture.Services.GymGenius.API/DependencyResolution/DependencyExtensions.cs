using CleanArchitecture.Services.GymGenius.Infrastructure.DependencyResolution;
using CleanArchitecture.Services.GymGenius.Application.DependencyResolution;
using FluentValidation;
using MediatR;

namespace CleanArchitecture.Services.GymGenius.API.DependencyResolution;

public static class DependencyExtensions
{
    public static IServiceCollection AddGymGenius(this IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddGymGeniusInfrastructure(environment, configuration);
        services.AddGymGeniusApplication();
        var assembly = typeof(DependencyExtensions).Assembly;
        services.AddAutoMapper(assembly);
        services.AddMediatR(assembly);
        services.AddValidatorsFromAssembly(assembly);

        //services.AddMasterSettingsAuthorizationRules();

        return services;
    }
}
