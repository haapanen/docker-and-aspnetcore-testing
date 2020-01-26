using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using IBusControl = ApplicationCore.Services.IBusControl;

namespace Infrastructure
{
    public class BusControl : IBusControl
    {
        private readonly MassTransit.IBusControl _busControl;

        public BusControl(MassTransit.IBusControl busControl)
        {
            _busControl = busControl;
        }

        public async Task Send<T>(T message, CancellationToken cancellationToken) where T : class
        {
            await _busControl.Send<T>(message, cancellationToken);
        }
    }
}
