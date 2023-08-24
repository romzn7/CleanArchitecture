using CleanArchitecture.Services.Shared.Infrastructure.Persistence;

namespace CleanArchitecture.Services.Shared.DomainDesign.SeedWork;

public interface IRepository<T>
where T : IQueryableEntity
{
    IUnitOfWork UnitOfWork { get; }
}

public interface IReadOnlyRepository<T> where T : IQueryableEntity
{ }
