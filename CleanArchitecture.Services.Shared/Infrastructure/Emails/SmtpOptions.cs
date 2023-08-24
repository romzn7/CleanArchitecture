namespace CleanArchitecture.Services.Shared.Infrastructure.Emails;

public class SmtpOptions
{
    public string Server { get; set; }
    public string NetworkCredentialUsername { get; set; }
    public string NetworkCredentiaPassword { get; set; }
    public int Port { get; set; }
    public string FromAddress { get; set; }
    public string DisplayNameForFromAddress { get; set; }
    public string ReplyToGlobal { get; set; }
    public string ReplyToGlobalName { get; set; }
}