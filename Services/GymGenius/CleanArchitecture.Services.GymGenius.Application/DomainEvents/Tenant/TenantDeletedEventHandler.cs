using CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.Events;

namespace CleanArchitecture.Services.GymGenius.Application.DomainEvents.Tenant;

internal class TenantDeletedEventHandler : INotificationHandler<TenantDeletedEvent>
{
    private readonly ILogger<TenantDeletedEventHandler> _logger;
    private readonly IEventLogRepository _eventLogRepository;

    public TenantDeletedEventHandler(ILoggerFactory logger,
        IEventLogRepository eventLogRepository)
    {
        _logger = logger.CreateLogger<TenantDeletedEventHandler>();
        _eventLogRepository = eventLogRepository;
    }

    public async Task Handle(TenantDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"GymGenius with Id:{notification.TenantId} has been deleted.");

        try
        {
            EventLog eventLog = new(Guid.NewGuid(), EventType.TenantDeleted, $"Deleted : {notification.UserId} deleted the Tenant {notification.name}-{notification.TenantId}.", notification.UserId);

            await _eventLogRepository.Add(eventLog, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{@notification}", notification);
            throw;
        }
    }
}
