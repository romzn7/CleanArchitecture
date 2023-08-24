using CleanArchitecture.Services.Shared.DomainDesign.SeedWork;

namespace CleanArchitecture.Services.Person.Application.Common.Repositories;

public interface IPersonRepository : IRepository<Domain.Aggregates.Person.Entities.Person>
{
    Task<Domain.Aggregates.Person.Entities.Person> UpdateAsync(Domain.Aggregates.Person.Entities.Person person, CancellationToken cancellationToken);
    Task<Domain.Aggregates.Person.Entities.Person> CreateAsync(Domain.Aggregates.Person.Entities.Person person, CancellationToken cancellationToken);
}

public interface IReadOnlyPersonRepository : IReadOnlyRepository<Domain.Aggregates.Person.Entities.Person>
{
    Task<bool> IsExist(string email, CancellationToken cancellationToken);
    Task<(IEnumerable<Domain.Aggregates.Person.Entities.Person> Results, int TotalRecords)> GetAll(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
}
