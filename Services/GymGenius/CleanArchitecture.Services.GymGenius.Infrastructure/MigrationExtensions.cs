﻿using CleanArchitecture.Services.Shared.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Services.GymGenius.Infrastructure;

public static class MigrationExtensions
{
    public static IHost UseGymGeniusMigration(this IHost app)
    {
        // configure seed
        app.MigrateDbContext<GymGeniusDbContext>((context, services) =>
        {
            var env = services.GetService<IWebHostEnvironment>();
            var settings = services.GetService<IOptions<GymGeniusInfrastructureSettings>>();
            var logger = services.GetService<ILogger<GymGeniusDbContextSeeding>>();

            if (settings?.Value?.EnableMigration ?? false)
                context.Database.Migrate();

            new GymGeniusDbContextSeeding()
                .SeedAsync(context, env, settings, logger)
                .Wait();
        });

        return app;
    }
}