using System.Text;
using Motto.Api;
using Motto.Entities;
using Motto.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class MotorcycleEventConsumer : BackgroundService
{
    private readonly ILogger<MotorcycleEventConsumer> _logger;
    private readonly RabbitMQService _rabbitMQService;
    private readonly IServiceScopeFactory _scopeFactory;
    private IConnection? _connection;
    private IModel? _channel;

    public MotorcycleEventConsumer(ILogger<MotorcycleEventConsumer> logger, RabbitMQService rabbitMQService, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _rabbitMQService = rabbitMQService;
        _scopeFactory = scopeFactory;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        // Crie uma conexão com o RabbitMQ
        _connection = _rabbitMQService.GetConnection();

        // Crie um canal
        _channel = _rabbitMQService.GetChannel();

        // Declare a troca (exchange)
        _channel.ExchangeDeclare(exchange: "motorcycle_events", type: ExchangeType.Direct);

        // Declare a fila
        var queueName = _channel.QueueDeclare().QueueName;

        // Faça o bind da fila com a troca
        _channel.QueueBind(queue: queueName,
                           exchange: "motorcycle_events",
                           routingKey: "motorcycle_registered");

        // Configure o consumidor
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (sender, ea) =>
        {
            // Receba a mensagem
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                // Recupere o contexto da base de dados
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Deserialize a mensagem para o objeto Motorcycle
                var messageObject = JsonConvert.DeserializeObject<Motorcycle>(message);

                // Faça algo com o objeto Motorcycle, como armazenar no banco de dados
                // Exemplo:
                var motorcycle = await dbContext.Motorcycles.FindAsync(messageObject?.Id);
                if (motorcycle != null)
                {
                    // Faça algo com o objeto Motorcycle recebido
                    _logger.LogInformation($"Moto cadastrada: {motorcycle}");
                }
                else
                {
                    _logger.LogWarning($"Moto não encontrada: {motorcycle}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar mensagem do RabbitMQ");
            }
        };

        // Registre o consumidor
        _channel.BasicConsume(queue: queueName,
                              autoAck: true,
                              consumer: consumer);

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Mantenha o serviço em execução enquanto não houver solicitação de cancelamento
        while (!stoppingToken.IsCancellationRequested)
        {
            // Aguarde um curto período antes de verificar novas mensagens
            await Task.Delay(1000, stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        // Feche o canal e a conexão ao parar o serviço
        _channel?.Close();
        _connection?.Close();

        await base.StopAsync(cancellationToken);
    }
}
