using RabbitMQ.Client;

namespace Motto.Services.Interfaces;

public interface IRabbitMQService
{
    IModel GetChannel();
    IConnection GetConnection();
}
