using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Polly.Retry;
using Polly;

namespace CleanArchitecture.Services.GymGenius.Infrastructure;

internal class GymGeniusDbContextSeeding
{
    public async Task SeedAsync(GymGeniusDbContext context, IWebHostEnvironment env, IOptions<GymGeniusInfrastructureSettings> settings,
    ILogger<GymGeniusDbContextSeeding> logger)
    {
        var policy = _CreatePolicy(logger, nameof(GymGeniusDbContextSeeding));

        await policy.ExecuteAsync(async () =>
        {
            using (context)
            {
                if (settings.Value.EnableMigrationSeed)
                {
                    await _MigrateEnumeration(context, context.EventTypes);
                    await _MigrateEnumeration(context, context.AcceptedPaymentMethods);
                    await _MigrateEnumeration(context, context.FacilityTypes);
                }
            }
        });
    }

    #region Helpers

    private async Task _MigrateEnumeration<T>(GymGeniusDbContext context, DbSet<T> entity)
        where T : Enumeration
    {
        var dbEnumerations = (await entity.ToListAsync()) ?? Enumerable.Empty<T>();
        var localEnumerations = Enumeration.GetAll<T>().Where(c => !dbEnumerations.Select(l => l.Id).Contains(c.Id));
        if (localEnumerations.Any())
        {
            foreach (var localEnumeration in localEnumerations)
                await context.AddAsync<T>(localEnumeration);

            await context.SaveChangesAsync();
        }
    }

    private AsyncRetryPolicy _CreatePolicy(ILogger<GymGeniusDbContextSeeding> logger, string prefix, int retries = 3) => Policy.Handle<SqlException>().
            WaitAndRetryAsync(
                retryCount: retries,
                sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                onRetry: (exception, timeSpan, retry, ctx) =>
                {
                    logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries} {TotalMilliseconds}", prefix, exception.GetType().Name, exception.Message, retry, retries, timeSpan.TotalMilliseconds);
                }
            );
    #endregion
}