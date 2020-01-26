using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Messages;
using ApplicationCore.Models;
using ApplicationCore.Services;
using MediatR;

namespace ApplicationCore.Handlers
{
    public class CreateMessageHandler : IRequestHandler<CreateMessage, MessageViewModel>
    {
        private readonly IBusControl _busControl;

        public CreateMessageHandler(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public async Task<MessageViewModel> Handle(CreateMessage request, CancellationToken cancellationToken)
        {
            await _busControl.Send(new Message
            {
                Date = DateTime.UtcNow,
                From = "Api",
                Text = request.Message.Text
            });

            return new MessageViewModel();
        }
    }
}
