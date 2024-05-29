using System.Text;
using Motto.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class MotorcycleEventConsumer : BackgroundService
{
    private readonly ILogger<MotorcycleEventConsumer> _logger;
    private IConnection? _connection;
    private IModel? _channel;
    private IConfiguration _configuration;

    public MotorcycleEventConsumer(ILogger<MotorcycleEventConsumer> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var connectionString = _configuration["RabbitMQ:ConnectionString"];
        if (string.IsNullOrEmpty(connectionString))
        {
            _logger.LogError("RabbitMQ connection string is not configured or is null");
            return;
        }

        var factory = new ConnectionFactory
        {
            Uri = new Uri(connectionString)
        };
        
        _logger.LogInformation($"RabbitMQ connection string is {connectionString}.");
        
        try {
            _connection =  factory.CreateConnection();
            _logger.LogInformation("RabbitMQ connected!");
        } catch(Exception ex) {
            _logger.LogError($"RabbitMQ not connected! {ex.Message}");
            return;
        }

        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: "motorcycle_events", type: ExchangeType.Direct);

        var queueName = _channel.QueueDeclare().QueueName;

        _channel.QueueBind(queue: queueName,
                           exchange: "motorcycle_events",
                           routingKey: "motorcycle_registered");

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (sender, ea) =>
        {
            // Receive message
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                // If want to use object to anything
                // var motorcycle = JsonConvert.DeserializeObject<Motorcycle>(message);

                _logger.LogInformation($"Moto cadastrada: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar mensagem do RabbitMQ");
            }
        };

        // Register consumer
        _channel.BasicConsume(queue: queueName,
                              autoAck: true,
                              consumer: consumer);

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _channel?.Close();
        _connection?.Close();

        await base.StopAsync(cancellationToken);
    }
}
