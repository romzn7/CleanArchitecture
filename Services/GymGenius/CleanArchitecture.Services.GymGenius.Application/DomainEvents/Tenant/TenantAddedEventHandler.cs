using CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.Events;

namespace CleanArchitecture.Services.GymGenius.Application.DomainEvents.GymGenius;

internal class TenantAddedEventHandler : INotificationHandler<TenantAddedEvent>
{
    private readonly ILogger<TenantAddedEventHandler> _logger;
    private readonly IEventLogRepository _eventLogRepository;

    public TenantAddedEventHandler(ILoggerFactory logger,
        IEventLogRepository eventLogRepository)
    {
        _logger = logger.CreateLogger<TenantAddedEventHandler>();
        _eventLogRepository = eventLogRepository;
    }

    public async Task Handle(TenantAddedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"GymGenius with Id:{notification.TenantId} has been created.");

        try
        {
            EventLog eventLog = new(Guid.NewGuid(), EventType.TenantAdded, $"Created : {notification.UserId} created the Tenant {notification.name}-{notification.TenantId}.", notification.UserId);

            await _eventLogRepository.Add(eventLog, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{@notification}", notification);
            throw;
        }
    }
}