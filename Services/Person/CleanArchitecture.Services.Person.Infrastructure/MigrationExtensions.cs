using Microsoft.Extensions.Hosting;
using CleanArchitecture.Services.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Services.Person.Infrastructure;

public static class MigrationExtensions
{
    public static IHost UsePersonMigration(this IHost app)
    {
        // configure seed
        app.MigrateDbContext<PersonDbContext>((context, services) =>
        {
            var env = services.GetService<IWebHostEnvironment>();
            var settings = services.GetService<IOptions<PersonInfrastructureSettings>>();
            var logger = services.GetService<ILogger<PersonDbContextSeeding>>();

            if (settings?.Value?.EnableMigration ?? false)
                context.Database.Migrate();

            new PersonDbContextSeeding()
                .SeedAsync(context, env, settings, logger)
                .Wait();
        });

        return app;
    }
}
