namespace CleanArchitecture.Services.Shared.Security;

public static class CustomClaimTypes
{
    public const string Agencies = "Agencies";
    public const string Entities = "entt";
    public const string Role = "Role";
    public const string EntityIds = "enttid";
}

public static class SkyNetJwtClaimTypes
{
    public const string SubjectType = "subt";
    public const string Tenant = "tnt";
}

public enum SubjectTypeEnum
{
    User,
    Static,
    External,
    Contact
}