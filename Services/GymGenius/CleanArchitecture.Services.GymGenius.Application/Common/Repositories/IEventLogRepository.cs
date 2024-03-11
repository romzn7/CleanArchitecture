using CleanArchitecture.Services.GymGenius.Domain.Aggregates.Events.Entities;
using CleanArchitecture.Services.Shared.DomainDesign.SeedWork;

namespace CleanArchitecture.Services.GymGenius.Application.Common.Repositories;
public interface IEventLogRepository : IRepository<EventLog>
{
    Task<EventLog> Add(EventLog eventLog, CancellationToken cancellationToken);
}
