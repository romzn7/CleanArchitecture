using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Services.Shared.Infrastructure.Emails;

public interface IEmailSenderService
{
    Task SendEmailAttachment(IEnumerable<string> toAddresses, string subject, string body, string fromAddress, IEnumerable<string> ccAddresses, IEnumerable<string> bccAddresses, bool isHtml, IDictionary<string, Stream> attachmentFiles, CancellationToken cancellationToken);
    Task SendEmailAttachment(IEnumerable<string> toAddresses, string subject, string body, string fromAddress = null, IEnumerable<string> ccAddresses = null, IEnumerable<string> bccAddresses = null, bool isHtml = false, IDictionary<string, Stream> attachmentFiles = null);
    Task SendEmail(IEnumerable<string> toAddresses, string subject, string body, string fromAddress = null, IEnumerable<string> ccAddresses = null, IEnumerable<string> bccAddresses = null, bool isHtml = false);
    Task SendEmail(MailMessage mailMessage);
}

public class EmailSenderService : IEmailSenderService
{
    private readonly ILogger<EmailSenderService> _logger;
    private readonly EmailServiceConfigurations _settings;

    public EmailSenderService(ILogger<EmailSenderService> logger, IOptions<EmailServiceConfigurations> settings)
    {
        this._logger = logger;
        this._settings = settings.Value;
    }


    public async Task SendEmail(IEnumerable<string> toAddresses, string subject, string body, string fromAddress = null, IEnumerable<string> ccAddresses = null, IEnumerable<string> bccAddresses = null, bool isHtml = false)
    {
        using var smtpClient = CreateSMTPClient();

        try
        {
            fromAddress = string.IsNullOrWhiteSpace(fromAddress) ? _settings.DefaultProfile.FromAddress : fromAddress;
            var message = new MailMessage
            {
                From = new MailAddress(fromAddress),
                Body = body,
                Subject = subject,
                IsBodyHtml = isHtml
            };

            if (!string.IsNullOrWhiteSpace(_settings.DefaultProfile.ReplyToAddress))
                message.ReplyToList.Add(new MailAddress(_settings.DefaultProfile.ReplyToAddress, _settings.DefaultProfile.ReplyToName));

            toAddresses.ToList().ForEach(e => message.To.Add(e));

            if (ccAddresses?.Any() ?? false)
                ccAddresses.ToList().ForEach(e => message.CC.Add(e));

            if (bccAddresses?.Any(e => !string.IsNullOrEmpty(e)) ?? false)
                bccAddresses.Where(e => !string.IsNullOrEmpty(e)).ToList().ForEach(e => message.Bcc.Add(e));

            await SendEmail(message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{@ToAddresses} {Subject} {Body} {FromAddress} {@CCAddresses}", toAddresses, subject, body,
                fromAddress, ccAddresses);
            throw;
        }
    }

    public async Task SendEmailAttachment(IEnumerable<string> toAddresses, string subject, string body, string fromAddress, IEnumerable<string> ccAddresses, IEnumerable<string> bccAddresses, bool isHtml, IDictionary<string, Stream> attachmentFiles, CancellationToken cancellationToken)
    {
        using var smtpClient = CreateSMTPClient();

        try
        {
            fromAddress = string.IsNullOrWhiteSpace(fromAddress) ? _settings.DefaultProfile.FromAddress : fromAddress;
            var message = new MailMessage
            {
                From = new MailAddress(fromAddress),
                Body = body,
                Subject = subject,
                IsBodyHtml = isHtml
            };

            toAddresses.ToList().ForEach(e => message.To.Add(e));

            if (ccAddresses?.Any() ?? false)
                ccAddresses.ToList().ForEach(e => message.CC.Add(e));

            if (bccAddresses?.Any(e => !string.IsNullOrEmpty(e)) ?? false)
                bccAddresses.Where(e => !string.IsNullOrEmpty(e)).ToList().ForEach(e => message.Bcc.Add(e));

            foreach (var attachmentFile in attachmentFiles ?? new Dictionary<string, Stream>())
                message.Attachments.Add(new Attachment(contentStream: attachmentFile.Value, name: attachmentFile.Key));

            if (!string.IsNullOrWhiteSpace(_settings.DefaultProfile.ReplyToAddress))
                message.ReplyToList.Add(new MailAddress(_settings.DefaultProfile.ReplyToAddress, _settings.DefaultProfile.ReplyToName));

            await SendEmail(message, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{@ToAddresses} {Subject} {Body} {FromAddress} {@CCAddresses} stream", toAddresses, subject, body,
                fromAddress, ccAddresses);
            throw;
        }
    }

    public async Task SendEmailAttachment(IEnumerable<string> toAddresses, string subject, string body, string fromAddress = null, IEnumerable<string> ccAddresses = null, IEnumerable<string> bccAddresses = null, bool isHtml = false, IDictionary<string, Stream> attachmentFiles = null)
        => await SendEmailAttachment(toAddresses, subject, body, fromAddress, ccAddresses, bccAddresses, isHtml, attachmentFiles, default(CancellationToken));

    public async Task SendEmail(MailMessage mailMessage)
     => await SendEmail(mailMessage, default(CancellationToken));

    public async Task SendEmail(MailMessage mailMessage, CancellationToken cancellationToken)
    {
        using var smtpClient = CreateSMTPClient();

        try
        {
            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
            await smtpClient.SendMailAsync(mailMessage, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{@MailMessage}", mailMessage);
            throw;
        }
    }

    private SmtpClient CreateSMTPClient() => new SmtpClient(_settings.SMTPSettings.Host, _settings.SMTPSettings.Port)
    {
        EnableSsl = false,
        DeliveryMethod = SmtpDeliveryMethod.Network,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(_settings.SMTPSettings.Username, _settings.SMTPSettings.Password),
        Timeout = _settings.SMTPSettings.TimeoutMilliseconds
    };
}

