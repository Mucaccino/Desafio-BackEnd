using RabbitMQ.Client;

namespace Motto.Domain.Services.Interfaces;

public interface IRabbitMQService
{
    IModel GetChannel();
    IConnection GetConnection();
}
