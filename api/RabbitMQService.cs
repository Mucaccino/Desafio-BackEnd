using System;
using RabbitMQ.Client;

namespace Motto.Api;

public class RabbitMQService
{
    private readonly string _connectionString;
    private IConnection? _connection;

    public RabbitMQService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Connect()
    {
        EnsureConnection();
    }
    
    public IConnection GetConnection()
    {
        EnsureConnection();
        return _connection!;
    }

    private void EnsureConnection()
    {
        if (_connection == null)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_connectionString)
            };

            _connection = factory.CreateConnection();
        }
    }
}
