using Microsoft.Extensions.DependencyInjection;
using Soenneker.Cloudflare.OriginCerts.Fetcher.Registrars;
using Soenneker.Managers.Runners.Registrars;

namespace Soenneker.Cloudflare.Runners.OriginCerts.Thumbprints;

/// <summary>
/// Console type startup
/// </summary>
public static class Startup
{
    // This method gets called by the runtime. Use this method to add services to the container.
    /// <summary>
    /// Configures services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static void ConfigureServices(IServiceCollection services)
    {
        services.SetupIoC();
    }

    /// <summary>
    /// Sets up io c.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The result of the operation.</returns>
    public static IServiceCollection SetupIoC(this IServiceCollection services)
    {
        services.AddHostedService<ConsoleHostedService>()
                .AddCloudflareOriginCertFetcherAsSingleton()
                .AddRunnersManagerAsSingleton();

        return services;
    }
}
