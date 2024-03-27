using MediatR;

namespace CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.Events;
public record TenantDeletedEvent(long TenantId, string name, int UserId) : INotification { }
