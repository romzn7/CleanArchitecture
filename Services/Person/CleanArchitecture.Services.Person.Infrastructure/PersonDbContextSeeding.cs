using CleanArchitecture.Services.Shared.DomainDesign.SeedWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Polly.Retry;
using Polly;

namespace CleanArchitecture.Services.Person.Infrastructure;

internal class PersonDbContextSeeding
{
    public async Task SeedAsync(PersonDbContext context, IWebHostEnvironment env, IOptions<PersonInfrastructureSettings> settings,
    ILogger<PersonDbContextSeeding> logger)
    {
        var policy = _CreatePolicy(logger, nameof(PersonDbContextSeeding));

        await policy.ExecuteAsync(async () =>
        {
            using (context)
            {
                if (settings.Value.EnableMigrationSeed)
                {
                    await _MigrateEnumeration(context, context.EventTypes);
                    await _MigrateEnumeration(context, context.Genders);

                }
            }
        });
    }

    #region Helpers

    private async Task _MigrateEnumeration<T>(PersonDbContext context, DbSet<T> entity)
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


    private AsyncRetryPolicy _CreatePolicy(ILogger<PersonDbContextSeeding> logger, string prefix, int retries = 3) => Policy.Handle<SqlException>().
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
