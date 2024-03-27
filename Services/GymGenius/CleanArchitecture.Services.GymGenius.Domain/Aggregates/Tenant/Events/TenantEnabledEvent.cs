namespace CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.Events;

public record TenantEnabledEvent(long TenantId, int UserId) : INotification { }
