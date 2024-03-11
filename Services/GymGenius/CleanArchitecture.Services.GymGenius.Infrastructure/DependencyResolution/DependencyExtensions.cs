using CleanArchitecture.Services.GymGenius.Infrastructure.Repositories;

namespace CleanArchitecture.Services.GymGenius.Infrastructure.DependencyResolution;

public static class DependencyExtensions
{
    public static IServiceCollection AddGymGeniusInfrastructure(this IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
    {
        services.Configure<GymGeniusInfrastructureSettings>(configuration.GetSection("GymGeniusSettings:Infrastructure"));

        services.AddDbContext<GymGeniusDbContext>(options => options.BuildReadOptimizedDbContext(environment, configuration.GetConnectionString(nameof(GymGeniusDbContext))!,
          typeof(DependencyExtensions).Assembly.GetName().Name!),
          ServiceLifetime.Scoped); //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
        services.AddDbContext<GymGeniusDbContext>(options => options.BuildReadOptimizedDbContext(environment, configuration.GetConnectionString(nameof(GymGeniusDbContext))),
        ServiceLifetime.Scoped);

        services.AddScoped<IEventLogRepository, EventLogRepository>();

        return services;
    }
}