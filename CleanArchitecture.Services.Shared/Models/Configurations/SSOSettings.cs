namespace CleanArchitecture.Services.Shared.Models.Configurations;

public class SSOSettings
{
    public string Authority { get; set; }
    public string ApiResourceName { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string SecurityProviderPath { get; set; }
    public string CookieDomain { get; set; }
    public string RedirectUri { get; set; }
}
