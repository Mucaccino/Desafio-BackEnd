using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using Motto.Api;

namespace Motto.Models
{
    public class MotorcycleEventProducer
    {
        private readonly RabbitMQService _rabbitMQService;

        public MotorcycleEventProducer(RabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }

        public void PublishMotorcycleRegisteredEvent(Motorcycle motorcycle)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(motorcycle));

            _rabbitMQService.GetChannel().BasicPublish(exchange: "motorcycle_events",
                                routingKey: "motorcycle_registered",
                                basicProperties: null,
                                body: body);
        }
    
        
    }
}
