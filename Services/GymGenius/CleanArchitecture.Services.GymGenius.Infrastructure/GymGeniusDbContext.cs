using CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.Entities;
using CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.Enumerations;
using CleanArchitecture.Services.Person.Infrastructure.EntityConfigurations;

namespace CleanArchitecture.Services.GymGenius.Infrastructure;

public class GymGeniusDbContext : DbContextBase<GymGeniusDbContext>
{
    public static string DEFAULT_SCHEMA => "gg";

    public GymGeniusDbContext(DbContextOptions<GymGeniusDbContext> options, ITimestampHelper currentDateTimeHelper, IMediator mediator, ILogger<GymGeniusDbContext> logger
        //ICurrentUserHelper currentUserHelper, 
        ) :
        base(options, currentDateTimeHelper, mediator, logger)
    {
        System.Diagnostics.Debug.WriteLine("GymGeniusDbContext::ctor ->" + this.GetHashCode());
    }

    public virtual DbSet<EventLog> EventLogs { get; set; } = null!;
    public virtual DbSet<EventType> EventTypes { get; set; } = null!;
    public virtual DbSet<FacilityType> FacilityTypes { get; set; } = null!;
    public virtual DbSet<AcceptedPaymentMethod> AcceptedPaymentMethods { get; set; } = null!;
    public virtual DbSet<Tenant> Tenants { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder
           .ApplyConfiguration(new EventLogEntityConfiguration())
           .ApplyConfiguration(new EventTypeEntityConfiguration())
           .ApplyConfiguration(new FacilityTypesEntityConfiguration())
           .ApplyConfiguration(new AcceptedPaymentMethodEntityConfiguration())
           ;
    }
}

