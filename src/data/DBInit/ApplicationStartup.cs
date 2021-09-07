using System;
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

        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting Database seed");
                await _seedService.SeedDatabase();
                _logger.LogInformation("Database seeded successfully");
            }
            catch (Exception e)
            {
                _logger.LogError("Database seed failed, Please fix the issue and try again. {0}", e);
            }

            _appLifetime.StopApplication();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Finished");

            return Task.CompletedTask;
        }
    }
}
