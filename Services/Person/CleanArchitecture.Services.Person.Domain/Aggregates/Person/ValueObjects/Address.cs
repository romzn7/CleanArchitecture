using Ardalis.GuardClauses;
using CleanArchitecture.Services.Shared.DomainDesign.SeedWork;
using System.Runtime.Versioning;

namespace CleanArchitecture.Services.Person.Domain.Aggregates.Person.ValueObjects;

public class Address : ValueObject
{

    protected Address()
    {

    }

    public Address(string city, string location, int wardNo)
    {
        City = Guard.Against.NullOrEmpty(city);
        Location = Guard.Against.NullOrEmpty(location);
        WardNo = Guard.Against.NegativeOrZero(wardNo);

    }

    public string City { get; private set; }
    public string Location { get; private set; }
    public int WardNo { get; private set; }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return City;
        yield return Location;
        yield return WardNo;
    }
}
