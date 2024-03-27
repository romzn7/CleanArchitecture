namespace CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.Enumerations;

public class AcceptedPaymentMethod : Enumeration, IAggregateRoot
{
    public static AcceptedPaymentMethod Cash = new AcceptedPaymentMethod(1, "Cash");
    public static AcceptedPaymentMethod Paypal = new AcceptedPaymentMethod(2, "Paypal");
    public static AcceptedPaymentMethod Card = new AcceptedPaymentMethod(3, "Card");
    public static AcceptedPaymentMethod Online = new AcceptedPaymentMethod(4, "Online");

    public AcceptedPaymentMethod(int id, string name) : base(id, name)
    {
    }
}