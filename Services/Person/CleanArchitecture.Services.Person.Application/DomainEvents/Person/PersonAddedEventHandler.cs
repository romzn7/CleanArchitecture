using CleanArchitecture.Services.Person.Application.Common.Repositories;
using CleanArchitecture.Services.Person.Domain.Aggregates.Events.Enumerations;
using CleanArchitecture.Services.Person.Domain.Aggregates.Person.Events;

namespace CleanArchitecture.Services.Person.Application.DomainEvents.Person;

internal class PersonAddedEventHandler : INotificationHandler<PersonAddedEvent>
{
    private readonly ILogger<PersonAddedEventHandler> _logger;
    private readonly IEventLogRepository _eventLogRepository;

    public PersonAddedEventHandler(ILoggerFactory logger,
        IEventLogRepository eventLogRepository)
    {
        _logger = logger.CreateLogger<PersonAddedEventHandler>();
        _eventLogRepository = eventLogRepository;
    }

    public async Task Handle(PersonAddedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogTrace($"Person with Id:{notification.PersonId} has been created.");

        try
        {
            EventLog eventLog = new(Guid.NewGuid(), EventType.PersonAdded, $"Created :  created the person {notification.name}-{notification.PersonGuid}.", notification.UserId);

            await _eventLogRepository.Add(eventLog, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{@notification}", notification);
            throw;
        }
    }
}
