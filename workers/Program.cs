var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<MotorcycleEventConsumer>();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var host = builder.Build();

// Console.WriteLine($"MottoWorkers (IsDevelopment: {host.)})");

host.Run();