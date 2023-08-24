namespace CleanArchitecture.Services.Shared.DomainDesign.SeedWork;

public interface ISharedEnumeration<T> where T : Enumeration
{
    static string Name { get; }
}

public interface ISharedEntity<T> where T : Entity
{
    static string Name { get; }
}
