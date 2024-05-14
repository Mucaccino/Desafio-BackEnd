using System;
using RabbitMQ.Client;

namespace Motto.Api;

public class RabbitMQService
{
    private readonly ILogger<RabbitMQService> _logger;
    private readonly string _connectionString;
    private IConnection? _connection;
    private IModel? _channel;

    public RabbitMQService(ILogger<RabbitMQService> logger, IConfiguration configuration)
    {
        _connectionString = configuration["RabbitMQ:ConnectionString"];
        _logger = logger;

        _logger.LogInformation($"RabbitMQ:ConnectionString {_connectionString}");

        Connect();
    }

    private void Connect()
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri(_connectionString)
        };
        
        try {
            _connection =  factory.CreateConnection();
            _logger.LogInformation("RabbitMQ connected!");
        } catch(Exception ex) {
            _logger.LogError($"RabbitMQ not connected! {ex.Message}");
            return;
        }
        
        _channel = _connection.CreateModel();
    }

    public IConnection? GetConnection() {
        return _connection;
    }
    
    public IModel? GetChannel() {
        return _channel;
    }
}
