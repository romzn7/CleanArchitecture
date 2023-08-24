using CleanArchitecture.Services.Person.Application.Common.Repositories;
using CleanArchitecture.Services.Person.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CleanArchitecture.Services.Person.Infrastructure.DependencyResolution;

public static class DependencyExtensions
{
    public static IServiceCollection AddPersonInfrastructure(this IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
    {
        services.Configure<PersonInfrastructureSettings>(configuration.GetSection("PersonSettings:Infrastructure"));

        services.AddDbContext<PersonDbContext>(options => options.BuildReadOptimizedDbContext(environment, configuration.GetConnectionString(nameof(PersonDbContext))!,
          typeof(DependencyExtensions).Assembly.GetName().Name!),
          ServiceLifetime.Scoped); //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
        services.AddDbContext<PersonDbContext>(options => options.BuildReadOptimizedDbContext(environment, configuration.GetConnectionString(nameof(PersonDbContext))),
        ServiceLifetime.Scoped);

        services.AddScoped<IEventLogRepository, EventLogRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IReadOnlyPersonRepository, ReadOnlyPersonRepository>();

        return services;
    }
}

