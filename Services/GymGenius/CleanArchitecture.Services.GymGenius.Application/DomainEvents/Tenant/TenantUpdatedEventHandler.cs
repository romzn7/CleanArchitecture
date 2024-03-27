using CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.Events;

namespace CleanArchitecture.Services.GymGenius.Application.DomainEvents.Tenant;

internal class TenantUpdatedEventHandler : INotificationHandler<TenantUpdatedEvent>
{
    private readonly ILogger<TenantUpdatedEventHandler> _logger;
    private readonly IEventLogRepository _eventLogRepository;

    public TenantUpdatedEventHandler(ILoggerFactory logger,
        IEventLogRepository eventLogRepository)
    {
        _logger = logger.CreateLogger<TenantUpdatedEventHandler>();
        _eventLogRepository = eventLogRepository;
    }

    public async Task Handle(TenantUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"GymGenius with Id:{notification.TenantId} has been updated.");

        try
        {
            EventLog eventLog = new(Guid.NewGuid(), EventType.TenantUpdated, $"Updated : {notification.UserId} updated the Tenant {notification.TenantId}.", notification.UserId);

            await _eventLogRepository.Add(eventLog, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{@notification}", notification);
            throw;
        }
    }
}