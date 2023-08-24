namespace CleanArchitecture.Services.Person.Infrastructure.Repositories;
public class EventLogRepository : IEventLogRepository
{
    private readonly ILogger<EventLogRepository> _logger;
    private readonly PersonDbContext _personDbContext;
    public EventLogRepository(ILogger<EventLogRepository> logger,
         PersonDbContext personDbContext)
    {
        _logger = logger;
        _personDbContext = personDbContext;
    }

    public IUnitOfWork UnitOfWork => _personDbContext;

    public async Task<EventLog> Add(EventLog eventLog, CancellationToken cancellationToken)
    {
        try
        {
            if (eventLog.IsTransient())
            {
                var entityEntry = await _personDbContext
                    .EventLogs
                    .AddAsync(eventLog);

                _personDbContext.Entry(entityEntry.Entity.EventType).State = EntityState.Detached;

                return entityEntry.Entity;
            }
            else
                return eventLog;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{@eventLog}", eventLog);
            throw;
        }
    }
}

