namespace CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.Events;

public record TenantDisabledEvent(long TenantId, int UserId) : INotification { }