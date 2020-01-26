using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Messages;
using ApplicationCore.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MessageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public Task<List<Message>> GetMessagesAsync()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("")]
        public async Task<MessageViewModel> CreateMessageAsync(NewMessageModel model)
        {
            var message = await _mediator.Send(new CreateMessage
            {
                Message = model
            });

            return message;
        }
    }
}