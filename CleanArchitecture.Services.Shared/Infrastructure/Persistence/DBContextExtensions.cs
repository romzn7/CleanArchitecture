using CleanArchitecture.Services.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace CleanArchitecture.Services.Shared.Infrastructure.Persistence;

public static class DBContextExtensions
{
    public static DbContextOptionsBuilder BuildDbContext(this DbContextOptionsBuilder builder, IHostEnvironment environment, string connectionString,
        string migrationsAssembly = null)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        builder.UseSqlServer(connectionString, sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);

            if (!string.IsNullOrWhiteSpace(migrationsAssembly))
                sqlOptions.MigrationsAssembly(migrationsAssembly);

            //Have to change the default schema of the migration table in order to prevent
            //production database of renaming the schema
            sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "efcore");
        });

        if (environment.IsDevelopmentEnvironment())
        {
            builder.EnableSensitiveDataLogging(true);
            builder.EnableDetailedErrors(true);
        }

        return builder;
    }

    public static DbContextOptionsBuilder BuildReadOptimizedDbContext(this DbContextOptionsBuilder builder, bool enableSentitiveLogging, string connectionString)
       => builder.BuildReadOptimizedDbContext(enableSentitiveLogging, connectionString, "");

    public static DbContextOptionsBuilder BuildReadOptimizedDbContext(this DbContextOptionsBuilder builder, IHostEnvironment environment, string connectionString)
       => builder.BuildReadOptimizedDbContext(environment.IsDevelopmentEnvironment(), connectionString, "");

    public static DbContextOptionsBuilder BuildReadOptimizedDbContext(this DbContextOptionsBuilder builder, IHostEnvironment environment, string connectionString,
        string migrationsAssembly)
        => builder.BuildReadOptimizedDbContext(environment.IsDevelopmentEnvironment(), connectionString, migrationsAssembly);

    public static DbContextOptionsBuilder BuildReadOptimizedDbContext(this DbContextOptionsBuilder builder, bool enableSentitiveLogging, string connectionString,
        string migrationsAssembly)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        builder.UseSqlServer(connectionString, sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);

            if (!string.IsNullOrWhiteSpace(migrationsAssembly))
                sqlOptions.MigrationsAssembly(migrationsAssembly);

            //Have to change the default schema of the migration table in order to prevent
            //production database of renaming the schema
            sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "efcore");

            //Force ef to use single sql inserts/updates instead of merge
            sqlOptions.MaxBatchSize(1);
        });

        if (enableSentitiveLogging)
        {
            builder.EnableSensitiveDataLogging(true);
            builder.EnableDetailedErrors(true);
        }

        //Disabled change tracking by default to optimize handling
        builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        return builder;
    }

    public static DbContextOptionsBuilder BuildTestingDbContext(this DbContextOptionsBuilder builder, string connectionString,
        string migrationsAssembly = null)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        builder.UseSqlServer(connectionString, sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);

            if (!string.IsNullOrWhiteSpace(migrationsAssembly))
                sqlOptions.MigrationsAssembly(migrationsAssembly);

            //Have to change the default schema of the migration table in order to prevent
            //production database of renaming the schema
            sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "efcore");

            //Force ef to use single sql inserts/updates instead of merge
            sqlOptions.MaxBatchSize(1);
        });

        //Disabled change tracking by default to optimize handling
        builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        return builder;
    }
}

