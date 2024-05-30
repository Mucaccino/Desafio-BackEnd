using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Motto.Services.Interfaces;
using RabbitMQ.Client;

namespace Motto.Services
{

    /// <summary>
    /// This class is responsible for managing the connection to the RabbitMQ service.
    /// It provides methods for connecting to the RabbitMQ server, getting the connection and channel objects.
    /// </summary>
    public class RabbitMQService : IRabbitMQService
    {
        private readonly ILogger<RabbitMQService> _logger;
        private readonly string _connectionString;
        private IConnection _connection;
        private IModel _channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMQService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        public RabbitMQService(ILogger<RabbitMQService> logger, IConfiguration configuration)
        {
            _connectionString = configuration["RabbitMQ:ConnectionString"] ?? "";
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

            try
            {
                _connection = factory.CreateConnection();
                _logger.LogInformation("RabbitMQ connected!");
                _channel = _connection.CreateModel();
            }
            catch (Exception ex)
            {
                _logger.LogError($"RabbitMQ not connected! {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the connection object.
        /// </summary>
        /// <returns>The connection object.</returns>
        public IConnection GetConnection() => _connection;

        /// <summary>
        /// Gets the channel object.
        /// </summary>
        /// <returns>The channel object.</returns>
        public IModel GetChannel() => _channel;
    }
}
