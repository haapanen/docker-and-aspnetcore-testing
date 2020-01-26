using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Messages;

namespace ApplicationCore.Services
{
    public interface IBusControl
    {
        Task Send<T>(T message, CancellationToken cancellationToken = default(CancellationToken)) where T : class;
    }
}
