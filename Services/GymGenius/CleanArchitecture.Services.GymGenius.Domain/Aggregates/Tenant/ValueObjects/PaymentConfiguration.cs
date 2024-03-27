
using CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.Enumerations;

namespace CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.ValueObjects;

public class PaymentConfiguration : ValueObject
{
    public List<AcceptedPaymentMethod> AcceptedPaymentMethods { get; private set; } = new List<AcceptedPaymentMethod>();

    protected PaymentConfiguration() { }
    public PaymentConfiguration(List<AcceptedPaymentMethod> acceptedPaymentMethods)
    {
        AcceptedPaymentMethods = Guard.Against.Null(acceptedPaymentMethods);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return AcceptedPaymentMethods;
    }
}
