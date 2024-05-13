using System;
using RabbitMQ.Client;

namespace Motto.Api;

public class RabbitMQService
{
    private readonly string _connectionString;
    private IConnection? _connection;
    private IModel? _channel;

    public RabbitMQService(string connectionString)
    {
        _connectionString = connectionString;

        Connect();
    }

    private void Connect()
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri(_connectionString)
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public IConnection? GetConnection() {
        return _connection;
    }
    
    public IModel? GetChannel() {
        return _channel;
    }
}
