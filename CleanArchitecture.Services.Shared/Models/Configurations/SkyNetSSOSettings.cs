namespace CleanArchitecture.Services.Shared.Models.Configurations;
public class CleanArchitectureSSOSettings
{
    public string Authority { get; set; }
    public string CommonRingPath { get; set; }
    public string Domain { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string RedirectUri { get; set; }
}
