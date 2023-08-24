using Ardalis.GuardClauses;
using CleanArchitecture.Services.Person.Domain.Aggregates.Events.Enumerations;
using CleanArchitecture.Services.Shared.DomainDesign.SeedWork;

namespace CleanArchitecture.Services.Person.Domain.Aggregates.Events.Entities;

public class EventLog : AuditableEntity, IAggregateRoot
{
    protected EventLog() { }
    public EventLog(Guid eventLogGUID, EventType eventType, string description, int userId)
    {
        Guard.Against.Default<Guid>(eventLogGUID);
        Guard.Against.Null(eventType);
        Guard.Against.NegativeOrZero(userId);
        Guard.Against.NullOrEmpty(description);

        this.EventLogGUID = eventLogGUID;
        EventType = eventType;
        this.AddedBy = userId;
        Description = description;
        this.IsActive = true;
        this.AddedOn = DateTime.UtcNow;
    }

    public Guid EventLogGUID { get; private init; }
    public EventType EventType { get; private init; }
    public string Description { get; private init; }
    public bool IsActive { get; private init; }
}
