namespace CleanArchitecture.Services.GymGenius.Infrastructure.Repositories;
public class EventLogRepository : IEventLogRepository
{
    private readonly ILogger<EventLogRepository> _logger;
    private readonly GymGeniusDbContext _gymGeniusDbContext;

    public EventLogRepository(ILogger<EventLogRepository> logger,
                            GymGeniusDbContext gymGeniusDbContext)
    {
        _logger = logger;
        _gymGeniusDbContext = gymGeniusDbContext;
    }
    public IUnitOfWork UnitOfWork => _gymGeniusDbContext;

    public async Task<EventLog> Add(EventLog eventLog, CancellationToken cancellationToken)
    {
        try
        {
            if (eventLog.IsTransient())
            {
                var entityEntry = await _gymGeniusDbContext
                    .EventLogs
                    .AddAsync(eventLog);

                _gymGeniusDbContext.Entry(entityEntry.Entity.EventType).State = EntityState.Detached;

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