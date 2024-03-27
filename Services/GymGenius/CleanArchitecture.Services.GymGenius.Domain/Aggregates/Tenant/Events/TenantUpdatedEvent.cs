using MediatR;

namespace CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.Events;
public record TenantUpdatedEvent(long TenantId, int UserId) : INotification { }
