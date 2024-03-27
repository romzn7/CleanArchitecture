namespace CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.Enumerations;

public class FacilityType : Enumeration, IAggregateRoot
{
    public static FacilityType Cardio = new FacilityType(1, "Cardio");
    public static FacilityType Gym = new FacilityType(2, "Gym");
    public static FacilityType Zumba = new FacilityType(3, "Zumba");
    public static FacilityType Yoga = new FacilityType(4, "Yoga");
    public static FacilityType Boxing = new FacilityType(5, "Boxing");
    public static FacilityType Swimming = new FacilityType(6, "Swimming");
    public static FacilityType Sauna = new FacilityType(7, "Sauna");
    public static FacilityType Steam = new FacilityType(8, "Steam");
    public FacilityType(int id, string name) : base(id, name)
    {
    }
}