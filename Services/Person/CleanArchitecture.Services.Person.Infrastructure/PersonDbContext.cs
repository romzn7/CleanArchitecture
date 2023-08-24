namespace CleanArchitecture.Services.Person.Infrastructure;
public class PersonDbContext : DbContextBase<PersonDbContext>
{
    public static string DEFAULT_SCHEMA => "prsn";

    public PersonDbContext(DbContextOptions<PersonDbContext> options, ITimestampHelper currentDateTimeHelper, IMediator mediator, ILogger<PersonDbContext> logger
        //ICurrentUserHelper currentUserHelper, 
        ) :
        base(options, currentDateTimeHelper, mediator, logger)
    {
        System.Diagnostics.Debug.WriteLine("PersonDbContext::ctor ->" + this.GetHashCode());
    }

    public virtual DbSet<EventLog> EventLogs { get; set; } = null!;
    public virtual DbSet<EventType> EventTypes { get; set; } = null!;
    public virtual DbSet<Gender> Genders { get; set; } = null!;
    public virtual DbSet<Domain.Aggregates.Person.Entities.Person> Persons { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder
           .ApplyConfiguration(new EventLogEntityConfiguration())
           .ApplyConfiguration(new EventTypeEntityConfiguration())
           .ApplyConfiguration(new PersonEntityConfiguration())
           .ApplyConfiguration(new GenderEntityConfiguration())
           ;
    }
}
