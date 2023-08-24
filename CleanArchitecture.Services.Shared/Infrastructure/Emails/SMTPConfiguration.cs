namespace CleanArchitecture.Services.Shared.Infrastructure.Emails;

public class SMTPConfiguration
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public int TimeoutMilliseconds { get; set; } = 30000;
}
