using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Motto.Api;
using Motto.Entities;
using Motto.Models;
using NSwag.Generation.Processors.Security;
using Motto.Utils;
using Serilog;
using Serilog.Exceptions;
using Motto.Services;
using Motto.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    Log.Information($"UseNpgsql (ConnectionString: {builder.Configuration.GetConnectionString("DefaultConnection")})");
});


builder.Logging.ClearProviders();

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration)
    // .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
    // .MinimumLevel.Override("Motto.Entities", Serilog.Events.LogEventLevel.Warning) 
);

// Configuration for authentication service
builder.Services.AddScoped<IAuthService, AuthService>(provider =>
{
    return new AuthService(provider.GetRequiredService<IUserRepository>(), new string(builder.Configuration["Jwt:Key"]), provider.GetRequiredService<ILogger<AuthService>>());
});

// Configuration for Swagger
builder.Services.AddSwaggerDocument(config =>
{
    config.Title = "MottoAPI";
    config.Version = "v1";
    config.Description = "API Documentation using NSwag";

    // Configuração do esquema de segurança
    config.AddSecurity("Bearer", new NSwag.OpenApiSecurityScheme
    {
        Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme (Ex: Bearer eyJhbGciOiJIUzI1NiIsI...)",
        In = NSwag.OpenApiSecurityApiKeyLocation.Header
    });

    // Configuração do requisito de segurança
    config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));

    // Adicione a descrição do esquema de segurança para a interface do usuário
    config.OperationProcessors.Add(new OperationSecurityScopeProcessor("Bearer"));
});

// Configuration for JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? ""))
        };
    });

// Configuration for authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole(UserType.Admin.ToString()));
    options.AddPolicy("DeliveryDriver", policy => policy.RequireRole(UserType.DeliveryDriver.ToString()));
});

// Configuration for Minio
builder.Services.AddSingleton<IMinioService>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new LicenseImageService(configuration);
});

// Add RabbitMQ Service
builder.Services.AddSingleton<RabbitMQService>();
// Add Event Producer
builder.Services.AddSingleton<MotorcycleEventProducer>();

// Adicionado consumidor de evento
// builder.Services.AddHostedService<MotorcycleEventConsumer>();

var app = builder.Build();

// Log information about the environment
Log.Information($"IsStaging: {app.Environment.IsStaging()}");

// Configure the HTTP request pipeline when is not Production mode
if (!app.Environment.IsProduction())
{    
    app.UseOpenApi();
    app.UseSwaggerUi();
    app.UseReDoc(options =>
    {
        options.Path = "/redoc";
    });
}


app.UseSerilogRequestLogging();
app.UseRouting();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

app.Run();

public partial class Program { }