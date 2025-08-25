using Microsoft.Extensions.DependencyInjection;
using Soenneker.Cloudflare.OriginCerts.Fetcher.Registrars;
using Soenneker.Cloudflare.Runners.OriginCerts.Thumbprints.Utils;
using Soenneker.Cloudflare.Runners.OriginCerts.Thumbprints.Utils.Abstract;
using Soenneker.Managers.Runners.Registrars;

namespace Soenneker.Cloudflare.Runners.OriginCerts.Thumbprints;

/// <summary>
/// Console type startup
/// </summary>
public static class Startup
{
    // This method gets called by the runtime. Use this method to add services to the container.
    public static void ConfigureServices(IServiceCollection services)
    {
        services.SetupIoC();
    }

    public static IServiceCollection SetupIoC(this IServiceCollection services)
    {
        services.AddHostedService<ConsoleHostedService>()
                .AddScoped<IFileOperationsUtil, FileOperationsUtil>()
                .AddCloudflareOriginCertFetcherAsScoped()
                .AddRunnersManagerAsScoped();

        return services;
    }
}
