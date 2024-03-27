namespace CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.ValueObjects;

public class Address : ValueObject
{
    public string Location { get; private set; }
    public string City { get; private set; }
    public string Country { get; private set; }
    public string PostalCode { get; private set; }

    protected Address() { }

    public Address(string location, string city, string country, string postalCode)
    {
        Location = Guard.Against.NullOrEmpty(location);
        City = Guard.Against.NullOrEmpty(city);
        Country = Guard.Against.NullOrEmpty(country);
        PostalCode = Guard.Against.NullOrEmpty(postalCode);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Location;
        yield return City;
        yield return Country;
        yield return PostalCode;
    }
}
