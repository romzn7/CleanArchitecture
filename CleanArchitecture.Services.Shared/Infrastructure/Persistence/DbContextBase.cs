using CleanArchitecture.Services.Shared.DomainDesign.SeedWork;
using CleanArchitecture.Services.Shared.Extensions;
using CleanArchitecture.Services.Shared.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Services.Shared.Infrastructure.Persistence;

public abstract class DbContextBase<TContext> : DbContext, IUnitOfWork where TContext : DbContext
{
    protected readonly ITimestampHelper TimestampHelper;
    protected readonly IMediator Mediator;
    protected readonly ILogger Logger;
    private readonly IgnoringIdentityResolutionInterceptor ignoringIdentityResolutionInterceptor = new();

    public DbContextBase(DbContextOptions<TContext> options,
        ITimestampHelper timestampHelper, IMediator mediator, ILogger logger)
        : base(options)
    {
        this.TimestampHelper = timestampHelper;
        this.Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.Logger = logger;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder
                .AddInterceptors(ignoringIdentityResolutionInterceptor);
    }

    public IUnitOfWork Reset()
    {
        this.ChangeTracker.Clear();
        return this;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<StringSplitResult>().ToTable(nameof(StringSplitResult), t => t.ExcludeFromMigrations(true));
    }

    public virtual async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                //var currentUser = await CurrentUserHelper.GetCurrentTrader();
                switch (entry.State)
                {
                    case EntityState.Added:
                        //entry.Entity.AddedBy = entry.Entity.AddedBy <= 0 ? currentUser?.Id ?? -1 : entry.Entity.AddedBy;
                        entry.Entity.AddedOn = TimestampHelper.GetCurrentUTC();
                        break;
                    case EntityState.Modified:
                        //entry.Entity.UpdatedBy = (currentUser?.Id ?? 0) <= 0 ? entry.Entity.UpdatedBy : currentUser?.Id;
                        entry.Entity.UpdatedOn = TimestampHelper.GetCurrentUTC();
                        break;
                }
            }

            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
            await Mediator.DispatchDomainEventsAsync(this);

            _EnsureEnumerationUnchanged();

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            var result = await base.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (DbUpdateConcurrencyException cEx)
        {
            Logger.LogError(cEx, cEx.Message);
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private void _EnsureEnumerationUnchanged()
    {
        //Enumerations should be changed in the db directly as a short term fix
        foreach (var entry in ChangeTracker.Entries<Enumeration>())
        {
            if (entry.State != EntityState.Unchanged)
                Logger.LogInformation("An attempt to add an enumeration {FullName} entity. This must be manually inserted into the db", entry.Entity.GetType().FullName);

            entry.State = EntityState.Unchanged;
        }
    }

    [Keyless]
    public class StringSplitResult
    {
        public string Value { get; set; }
    }

    [DbFunction(IsBuiltIn = true, Name = "STRING_SPLIT")]
    public IQueryable<StringSplitResult> Split(string source, string separator)
        => FromExpression(() => Split(source, separator));
}

