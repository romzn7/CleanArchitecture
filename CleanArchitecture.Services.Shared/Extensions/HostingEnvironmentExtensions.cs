using Microsoft.Extensions.Hosting;

namespace CleanArchitecture.Services.Shared.Extensions;

public static class HostingEnvironmentExtensions
{
    /// <summary>
    /// Checks if the current hosting environment name starts with <see cref="Environments.Development"/>
    /// </summary>
    /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
    /// <returns>True if the environment name starts with <see cref="Environments.Development"/>, otherwise false.</returns>
    public static bool IsDevelopmentEnvironment(this IHostEnvironment hostEnvironment)
    {
        if (hostEnvironment == null)
        {
            throw new ArgumentNullException(nameof(hostEnvironment));
        }

        return hostEnvironment.EnvironmentName?.StartsWith(Environments.Development, StringComparison.InvariantCultureIgnoreCase) ?? hostEnvironment.IsDevelopment();
    }
}