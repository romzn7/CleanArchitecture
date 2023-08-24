using CleanArchitecture.Services.Person.Application.Common.Repositories;
using CleanArchitecture.Services.Shared.DomainDesign.SeedWork;

namespace CleanArchitecture.Services.Person.Infrastructure.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly ILogger<PersonRepository> _logger;
    private readonly PersonDbContext _personDbContext;

    public PersonRepository(ILogger<PersonRepository> logger,
        PersonDbContext personDbContext)
    {
        _logger = logger;
        _personDbContext=personDbContext;
    }
    public IUnitOfWork UnitOfWork => _personDbContext;

    public async Task<Domain.Aggregates.Person.Entities.Person> CreateAsync(Domain.Aggregates.Person.Entities.Person person, CancellationToken cancellationToken)
    {
        try
        {
            if (person.IsTransient())
            {
                var personEntity = await _personDbContext
                                 .Persons
                                 .AddAsync(person, cancellationToken);

                return personEntity.Entity;
            }
            else return person;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{@person}", person);
            throw;
        }
    }

    public async Task<Domain.Aggregates.Person.Entities.Person> UpdateAsync(Domain.Aggregates.Person.Entities.Person person, CancellationToken cancellationToken)
    {
        try
        {
            if (!person.IsTransient())
            {

                return _personDbContext
                .Persons
                .Update(person)
                .Entity;
            }
            else return person;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{@person}", person);
            throw;
        }
    }

}



public class ReadOnlyPersonRepository : IReadOnlyPersonRepository
{
    private readonly ILogger<ReadOnlyPersonRepository> _logger;
    private readonly PersonDbContext _personDbContext;
    public ReadOnlyPersonRepository(ILogger<ReadOnlyPersonRepository> logger,
        PersonDbContext personDbContext)
    {
        _logger = logger;
        _personDbContext = personDbContext;
    }

    public async Task<bool> IsExist(string email, CancellationToken cancellationToken)
    {
        try
        {
            return await _personDbContext
                         .Persons
                         .AsNoTracking()
                         .AnyAsync(x => x.Email.Equals(email)
                                  && x.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{@email}", email);
            throw;
        }
    }


    public async Task<(IEnumerable<Domain.Aggregates.Person.Entities.Person> Results, int TotalRecords)> GetAll(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
    {
        try
        {
            var query = _personDbContext.Persons
                 .AsNoTracking()
                 .Where(x => x.IsActive)
                 .Include(x => x.Address)
                 .Include(x => x.Gender)
                 .OrderBy(x => x.Name)
                 .AsSplitQuery()
                 .AsQueryable();

            query = !string.IsNullOrEmpty(searchTerm) ? query.Where(x => x.Name.Contains(searchTerm)) : query;

            int totalRecords = await query.CountAsync(cancellationToken);

            var results = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (results, totalRecords);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{@pageNumber} {@pageSize} {@searchTerm}", pageNumber, pageSize, searchTerm);
            throw;
        }
    }
}
