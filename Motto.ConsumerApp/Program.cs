using Motto.ConsumerApp.Consumers;
using Serilog;

namespace Motto.ConsumerApp;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddHostedService<MotorcycleEventConsumer>();

        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

        builder.Services.AddSerilog((services, loggerConfiguration) => loggerConfiguration
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console());

        var host = builder.Build();

        host.Run();
    }
}