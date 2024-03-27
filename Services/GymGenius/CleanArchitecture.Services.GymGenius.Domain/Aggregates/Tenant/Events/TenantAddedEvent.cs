namespace CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.Events;
public record TenantAddedEvent(long TenantId, Guid TenantGuid, string name, int UserId) : INotification { }
