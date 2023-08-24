using CleanArchitecture.Services.Shared.DomainDesign.SeedWork;
using Humanizer;

namespace CleanArchitecture.Services.Person.Domain.Aggregates.Events.Enumerations;

public class EventType : Enumeration, IAggregateRoot
{
    #region Media Type Event Types
    public static readonly EventType PersonAdded = new EventType(1, nameof(PersonAdded).Humanize().Titleize());
    public static readonly EventType PersonDeleted = new EventType(2, nameof(PersonDeleted).Humanize().Titleize());
    public static readonly EventType PersonUpdated = new EventType(3, nameof(PersonUpdated).Humanize().Titleize());
    #endregion

    public EventType(int id, string name) : base(id, name)
    {
    }
}
