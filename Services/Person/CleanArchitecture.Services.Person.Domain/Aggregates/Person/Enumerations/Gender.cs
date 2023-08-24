using CleanArchitecture.Services.Shared.DomainDesign.SeedWork;

namespace CleanArchitecture.Services.Person.Domain.Aggregates.Person.Enumerations;

public class Gender : Enumeration, IAggregateRoot
{
    public static readonly Gender Male = new Gender(1, nameof(Male));
    public static readonly Gender Female = new Gender(2, nameof(Female));
    public static readonly Gender Others = new Gender(3, nameof(Others));
    public Gender(int id, string name) : base(id, name)
    {
    }
}
