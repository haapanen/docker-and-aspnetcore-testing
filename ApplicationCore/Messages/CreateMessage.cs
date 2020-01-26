using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Models;
using MediatR;

namespace ApplicationCore.Messages
{
    public class CreateMessage : IRequest<MessageViewModel>
    {
        public NewMessageModel Message { get; set; }
    }
}
