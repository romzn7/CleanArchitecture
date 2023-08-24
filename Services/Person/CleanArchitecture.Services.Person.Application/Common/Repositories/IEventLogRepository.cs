namespace CleanArchitecture.Services.Person.Application.Common.Repositories;
public interface IEventLogRepository : IRepository<EventLog>
{
    Task<EventLog> Add(EventLog eventLog, CancellationToken cancellationToken);
}
