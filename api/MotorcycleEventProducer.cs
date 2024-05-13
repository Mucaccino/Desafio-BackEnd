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
            // Crie uma conexão com o RabbitMQ
            using var connection = _rabbitMQService.GetConnection();
            
            // Crie um canal
            using var channel = connection.CreateModel();
            
            // Declare a troca (exchange)
            channel.ExchangeDeclare(exchange: "motorcycle_events", type: ExchangeType.Direct);

            // Serialize os dados do evento
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(motorcycle));

            // Publique a mensagem no RabbitMQ
            channel.BasicPublish(exchange: "motorcycle_events",
                                 routingKey: "motorcycle_registered",
                                 basicProperties: null,
                                 body: body);
        }
    }
}
