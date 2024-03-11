using CleanArchitecture.Services.Shared.DomainDesign.SeedWork;
using Humanizer;

namespace CleanArchitecture.Services.GymGenius.Domain.Aggregates.Events.Enumerations;

public class EventType : Enumeration, IAggregateRoot
{
    #region Media Type Event Types
    public static readonly EventType TenantAdded = new EventType(1, nameof(TenantAdded).Humanize().Titleize());
    public static readonly EventType TenantDeleted = new EventType(2, nameof(TenantDeleted).Humanize().Titleize());
    public static readonly EventType TenantUpdated = new EventType(3, nameof(TenantUpdated).Humanize().Titleize());
    #endregion

    public EventType(int id, string name) : base(id, name)
    {
    }
}
