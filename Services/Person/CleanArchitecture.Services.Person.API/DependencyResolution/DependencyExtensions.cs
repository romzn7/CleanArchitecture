using CleanArchitecture.Services.Person.Application.DependencyResolution;
using CleanArchitecture.Services.Person.Infrastructure.DependencyResolution;
using FluentValidation;
using MediatR;

namespace CleanArchitecture.Services.Person.API.DependencyResolution;

public static class DependencyExtensions
{
    public static IServiceCollection AddPerson(this IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddPersonInfrastructure(environment, configuration);
        services.AddPersonApplication();
        var assembly = typeof(DependencyExtensions).Assembly;
        services.AddAutoMapper(assembly);
        services.AddMediatR(assembly);
        services.AddValidatorsFromAssembly(assembly);

        //services.AddMasterSettingsAuthorizationRules();

        return services;
    }
}
