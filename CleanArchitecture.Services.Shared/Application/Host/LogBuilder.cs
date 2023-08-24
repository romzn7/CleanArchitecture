using Microsoft.Extensions.Hosting;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using CleanArchitecture.Services.Shared.Extensions;
using Microsoft.Extensions.Configuration;
using NewRelic.LogEnrichers.Serilog;

namespace CleanArchitecture.Services.Shared.Application.Host;

public static class Logging
{
    public static Action<HostBuilderContext, LoggerConfiguration> ConfigureSerilogger =>
        (hostingContext, loggerConfiguration) =>
        {
            var env = hostingContext.HostingEnvironment;

            loggerConfiguration.MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty(nameof(env.ApplicationName), env.ApplicationName)
                .Enrich.WithProperty(nameof(env.EnvironmentName), env.EnvironmentName)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .WriteTo.Console();

            if (hostingContext.HostingEnvironment.IsDevelopmentEnvironment())
            {
                loggerConfiguration.MinimumLevel.Override("SkyNet.Integrations.Omniflow", LogEventLevel.Debug)
                    .MinimumLevel.Override("SkyNet.Services.Omniflow", LogEventLevel.Debug)
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
                    .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Verbose)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Verbose);
            }
            else
            {
                loggerConfiguration.MinimumLevel.Override("SkyNet.Integrations.Omniflow", LogEventLevel.Information)
                    .MinimumLevel.Override("SkyNet.Services.Omniflow", LogEventLevel.Information);
            }

            var elasticSearchUrl = hostingContext.Configuration.GetValue<string>("Logging:ElasticSearchUrl");

            if (!string.IsNullOrEmpty(elasticSearchUrl))
            {
                loggerConfiguration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearchUrl))
                {
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                    IndexFormat = "CleanArchitecture-Api-Logs-{0:yyyy.MM.dd}",
                    MinimumLogEventLevel = LogEventLevel.Debug
                });
            }

            var fileLogging = hostingContext.Configuration.GetValue<string>("Logging:LogFilePath");

            if (!string.IsNullOrEmpty(fileLogging))
            {
                loggerConfiguration.WriteTo.Logger(lc => lc.Filter.ByExcluding("Contains(SourceContext, 'Omniflow')")
                    .WriteTo.File($"{fileLogging}\\Log.log", rollingInterval: RollingInterval.Day, shared: true,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"));

            }

            var newRelicLogFilePath = hostingContext.Configuration.GetValue<string>("Logging:NewRelicLogFilePath");

            if (!string.IsNullOrEmpty(newRelicLogFilePath))
            {
                loggerConfiguration
                    .Enrich.WithNewRelicLogsInContext()
                    .WriteTo.File(formatter: new NewRelicFormatter(), path: $"{newRelicLogFilePath}\\NewRelicLog.log.json", rollingInterval: RollingInterval.Day, shared: true);
            }
        };
}
