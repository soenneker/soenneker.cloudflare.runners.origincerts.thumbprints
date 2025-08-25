using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Soenneker.Cloudflare.OriginCerts.Fetcher.Abstract;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using Soenneker.Managers.Runners.Abstract;
using Soenneker.Utils.File.Abstract;
using Soenneker.Utils.Path.Abstract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Cloudflare.Runners.OriginCerts.Thumbprints;

public sealed class ConsoleHostedService : IHostedService
{
    private readonly ILogger<ConsoleHostedService> _logger;

    private readonly IHostApplicationLifetime _appLifetime;
    private readonly IRunnersManager _runnersManager;
    private readonly ICloudflareOriginCertFetcher _cloudflareOriginCertFetcher;
    private readonly IFileUtil _fileUtil;
    private readonly IPathUtil _pathUtil;

    private int? _exitCode;

    public ConsoleHostedService(ILogger<ConsoleHostedService> logger, IHostApplicationLifetime appLifetime, IRunnersManager runnersManager,
        ICloudflareOriginCertFetcher cloudflareOriginCertFetcher, IFileUtil fileUtil, IPathUtil pathUtil)
    {
        _logger = logger;
        _appLifetime = appLifetime;
        _runnersManager = runnersManager;
        _cloudflareOriginCertFetcher = cloudflareOriginCertFetcher;
        _fileUtil = fileUtil;
        _pathUtil = pathUtil;
    }

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        _appLifetime.ApplicationStarted.Register(() =>
        {
            Task.Run(async () =>
            {
                _logger.LogInformation("Running console hosted service ...");

                try
                {
                    List<string> thumbprints = await _cloudflareOriginCertFetcher.GetSharedAopThumbprints(cancellationToken).NoSync();

                    string tempFilePath = await _pathUtil.GetRandomTempFilePath(".txt", cancellationToken).NoSync();

                    await _fileUtil.WriteAllLines(tempFilePath, thumbprints, true, cancellationToken).NoSync();

                    await _runnersManager.PushIfChangesNeeded(tempFilePath, Constants.FileName, Constants.Library,
                        $"https://github.com/soenneker/{Constants.Library}", cancellationToken);

                    _logger.LogInformation("Complete!");

                    _exitCode = 0;
                }
                catch (Exception e)
                {
                    if (Debugger.IsAttached)
                        Debugger.Break();

                    _logger.LogError(e, "Unhandled exception");

                    await Task.Delay(2000, cancellationToken);
                    _exitCode = 1;
                }
                finally
                {
                    // Stop the application once the work is done
                    _appLifetime.StopApplication();
                }
            }, cancellationToken);
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Exiting with return code: {exitCode}", _exitCode);

        // Exit code may be null if the user cancelled via Ctrl+C/SIGTERM
        Environment.ExitCode = _exitCode.GetValueOrDefault(-1);
        return Task.CompletedTask;
    }
}