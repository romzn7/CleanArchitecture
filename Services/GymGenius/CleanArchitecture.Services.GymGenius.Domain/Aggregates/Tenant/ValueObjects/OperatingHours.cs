namespace CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.ValueObjects;

public class OperatingHours : ValueObject
{
    public TimeOnly OpeningTime { get; private set; }
    public TimeOnly ClosingTime { get; private set; }

    protected OperatingHours() { }


    public OperatingHours(TimeOnly openingTime, TimeOnly closingTime)
    {
        OpeningTime = Guard.Against.Default(openingTime);
        ClosingTime = Guard.Against.Default(closingTime);
        
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return OpeningTime;
        yield return ClosingTime;
    }
}
