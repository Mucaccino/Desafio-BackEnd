using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using Motto.Models;

namespace Motto.Services.EventProducers
{
    public class MotorcycleEventProducer
    {
        private readonly IRabbitMQService _rabbitMQService;

        public MotorcycleEventProducer(IRabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }

        public void PublishMotorcycleRegisteredEvent(Motorcycle motorcycle)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(motorcycle));

            _rabbitMQService.GetChannel().BasicPublish(
                exchange: "motorcycle_events",
                routingKey: "motorcycle_registered",
                basicProperties: null,
                body: body);
        }
    }
}
