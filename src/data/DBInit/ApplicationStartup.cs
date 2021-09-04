using System.Threading;
using System.Threading.Tasks;
using DBInit.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DBInit
{
    public class ApplicationStartup : IHostedService
    {
        private readonly ILogger<ApplicationStartup> _logger;
        private readonly ISeedService _seedService;
        private readonly IHostApplicationLifetime _appLifetime;

        public ApplicationStartup(ILogger<ApplicationStartup> logger, ISeedService seedService, IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
            _seedService = seedService;
            _appLifetime = appLifetime;
            // appLifetime.ApplicationStarted.Register(OnStarted);

        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("1. StartAsync has been called.");

            _appLifetime.StopApplication();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("4. StopAsync has been called.");

            return Task.CompletedTask;
        }
    }
}
