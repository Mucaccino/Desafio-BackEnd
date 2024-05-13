using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Motto.Models;
using Motto.Utils;
using Motto.Api;
using Motto.Entities;

public class MotorcycleEventConsumer : BackgroundService
{
    private readonly ILogger<MotorcycleEventConsumer> _logger;
    private readonly RabbitMQService _rabbitMQService;
    private readonly IServiceScopeFactory _scopeFactory;

    public MotorcycleEventConsumer(ILogger<MotorcycleEventConsumer> logger, RabbitMQService rabbitMQService, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _rabbitMQService = rabbitMQService;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        { 
            // Recupere o contexto da base de dados
            using var scope = _scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Crie uma conexão com o RabbitMQ
            using var connection = _rabbitMQService.GetConnection();
            
            // Crie um canal
            using var channel = connection.CreateModel();
            
            // Declare a troca (exchange)
            channel.ExchangeDeclare(exchange: "motorcycle_events", type: ExchangeType.Direct);
            
            // Declare a fila
            var queueName = channel.QueueDeclare().QueueName;
            
            // Faça o bind da fila com a troca
            channel.QueueBind(queue: queueName,
                              exchange: "motorcycle_events",
                              routingKey: "motorcycle_registered");

            // Configure o consumidor
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (sender, ea) =>
            {
                // Receba a mensagem
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    // Deserialize a mensagem para o objeto Motorcycle
                    var motorcycleId = JsonConvert.DeserializeObject<int>(message);
                    
                    // Faça algo com o objeto Motorcycle, como armazenar no banco de dados
                    // Exemplo:
                    var motorcycle = await _dbContext.Motorcycles.FindAsync(motorcycleId);
                    if (motorcycle != null)
                    {
                        // Faça algo com o objeto Motorcycle recebido
                        _logger.LogInformation($"Moto cadastrada: {motorcycleId}");
                    }
                    else
                    {
                        _logger.LogWarning($"Moto não encontrada: {motorcycleId}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar mensagem do RabbitMQ");
                }
            };

            // Registre o consumidor
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

            await Task.Delay(1000, stoppingToken); // Aguarde 1 segundo antes de verificar novas mensagens
        }
    }
}
