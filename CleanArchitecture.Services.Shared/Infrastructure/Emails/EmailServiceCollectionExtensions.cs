using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Services.Shared.Infrastructure.Emails;

public static class EmailServiceCollectionExtensions
{
    public static IServiceCollection AddEmailSenderService(this IServiceCollection services, Action<EmailServiceConfigurations> configuration)
    {
        if (configuration == null)
            throw new InvalidOperationException("Configuration missing");

        services.AddTransient<IEmailSenderService, EmailSenderService>();
        services.Configure(configuration);

        return services;
    }

    public static IServiceCollection AddEmailSenderService(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration == null)
            throw new InvalidOperationException("Configuration missing");

        var smtpSettings = configuration.GetSection("Smtp").Get<SmtpOptions>();

        services.Configure<SmtpOptions>(configuration.GetSection("Smtp"));

        if (smtpSettings == null)
            throw new InvalidOperationException("Configuration missing for smtp");

        services.AddTransient<IEmailSenderService, EmailSenderService>();
        services.Configure<EmailServiceConfigurations>(c =>
        {
            c.SMTPSettings = new SMTPConfiguration
            {
                Host = smtpSettings.Server,
                Port = smtpSettings.Port,
                Password = smtpSettings.NetworkCredentiaPassword,
                Username = smtpSettings.NetworkCredentialUsername
            };
            c.DefaultProfile = new EmailProfile
            {
                FromAddress = smtpSettings.FromAddress,
                ReplyToAddress = smtpSettings.ReplyToGlobal,
                ReplyToName = smtpSettings.ReplyToGlobalName
            };
        });

        return services;
    }
}

