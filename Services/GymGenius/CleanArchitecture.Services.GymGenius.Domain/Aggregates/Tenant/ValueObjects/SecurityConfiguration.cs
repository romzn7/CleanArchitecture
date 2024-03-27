namespace CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.ValueObjects;

public class SecurityConfiguration : ValueObject
{
    public bool RequireAccessCard { get; private set; }
    public bool BiometricAuthentication { get; private set; }

    public SecurityConfiguration(bool requireAccessCard, bool biometricAuthentication)
    {
        RequireAccessCard = Guard.Against.Default(requireAccessCard);
        BiometricAuthentication = Guard.Against.Default(biometricAuthentication);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return RequireAccessCard;
        yield return BiometricAuthentication;
    }
}
