using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ApplicationCore;
using MassTransit;
using Newtonsoft.Json;

namespace BackgroundService
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        private static async Task RunAsync()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host("rabbitmq://rabbitmq", cfg =>
                {
                    cfg.Username("guest");
                    cfg.Password("guest");
                });

                EndpointConvention.Map<Acknowledgement>(new Uri("rabbitmq://rabbitmq/message-acks"));

                sbc.ReceiveEndpoint("messages", cfg =>
                {
                    cfg.Handler<Message>(async(context) =>
                    {
                        await Console.Out.WriteLineAsync($"Received {context.Message.Text}");

                        await context.Send(new Acknowledgement
                        {
                            Id = context.Message.Text
                        });
                    });
                });
            });

            await busControl.StartAsync();

            Console.ReadLine();

            await busControl.StopAsync();
        }
    }
}
