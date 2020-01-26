using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Api.Controllers;
using ApplicationCore;
using Infrastructure;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMediatR(typeof(MessageController).Assembly, typeof(Message).Assembly);
            ConfigureServiceBus(services);
        }

        private void ConfigureServiceBus(IServiceCollection services)
        {
            IBusControl CreateBus(IServiceProvider serviceProvider)
            {
                return Bus.Factory.CreateUsingRabbitMq(sbc =>
                {
                    sbc.Host("rabbitmq://rabbitmq", cfg =>
                    {
                        cfg.Username("guest");
                        cfg.Password("guest");
                    });
                    sbc.ExchangeType = ExchangeType.Direct;

                    EndpointConvention.Map<Message>(new Uri("rabbitmq://rabbitmq/messages"));

                    sbc.ReceiveEndpoint("message-acks", cfg =>
                    {
                        cfg.Handler<Acknowledgement>(context =>
                        {
                            return Console.Out.WriteLineAsync($"Acknowledged {context.Message.Id}");
                        });

                        cfg.Consumer(() => serviceProvider.GetService<AcknowledgementConsumer>());
                        cfg.Consumer(() => serviceProvider.GetService<AcknowledgementFaultConsumer>());
                    });
                });
            }

            services.AddMassTransit(cfg =>
            {
                cfg.AddBus(CreateBus);
            });

            services.AddTransient<ApplicationCore.Services.IBusControl, BusControl>();
            services.AddSingleton<IHostedService, QueueManagerConsoleHostedService>();
            services.AddTransient<AcknowledgementConsumer>();
            services.AddTransient<AcknowledgementFaultConsumer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    internal class AcknowledgementFaultConsumer : IConsumer<Fault<Acknowledgement>>
    {
        public Task Consume(ConsumeContext<Fault<Acknowledgement>> context)
        {
            return Task.CompletedTask;
        }
    }

    internal class AcknowledgementConsumer : IConsumer<Acknowledgement>
    {
        private readonly ILogger _logger;

        public AcknowledgementConsumer(ILoggerFactory factory)
        {
            _logger = factory.CreateLogger<AcknowledgementConsumer>();
        }

        public Task Consume(ConsumeContext<Acknowledgement> context)
        {
            _logger.LogInformation("Here");

            throw new NotImplementedException();

            return Console.Out.WriteLineAsync("AckConsumer");
        }
    }
}
