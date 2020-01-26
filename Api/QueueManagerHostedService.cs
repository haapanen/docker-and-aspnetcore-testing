using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class QueueManagerConsoleHostedService :
        IHostedService
    {
        readonly IBusControl _bus;
        readonly ILogger _logger;

        public QueueManagerConsoleHostedService(IBusControl bus, ILoggerFactory loggerFactory)
        {
            _bus = bus;
            _logger = loggerFactory.CreateLogger<QueueManagerConsoleHostedService>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting bus");
            await _bus.StartAsync(cancellationToken).ConfigureAwait(false);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping bus");
            return _bus.StopAsync(cancellationToken);
        }
    }
}
