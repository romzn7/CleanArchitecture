namespace CleanArchitecture.Services.Shared.Infrastructure.Emails;
public class EmailServiceConfigurations
{
    public EmailProfile DefaultProfile { get; set; } = new EmailProfile();
    public SMTPConfiguration SMTPSettings { get; set; } = new SMTPConfiguration();
}
