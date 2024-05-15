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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Logging.ClearProviders();

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration)
    // .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
    // .MinimumLevel.Override("Motto.Entities", Serilog.Events.LogEventLevel.Warning) 
);

// Configure NSwag


// Configuração do Swagger
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

// Configure JWT

var jwtKey = new string(builder.Configuration["Jwt:Key"]);
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole(UserType.Admin.ToString()));
    options.AddPolicy("DeliveryDriver", policy => policy.RequireRole(UserType.DeliveryDriver.ToString()));
});

builder.Services.AddSingleton(jwtKey);

// Configuração do MinIO
builder.Services.AddSingleton<IMinioService>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new LicenseImageService(configuration);
});

// Adicione o serviço RabbitMQ ao contêiner de serviços
builder.Services.AddSingleton<RabbitMQService>();


builder.Services.AddSingleton<MotorcycleEventProducer>();

// Adicionado consumidor de evento
// builder.Services.AddHostedService<MotorcycleEventConsumer>();

var app = builder.Build();

// Console.WriteLine($"MottoAPI (IsDevelopment: {app.Environment.IsDevelopment()})");
Log.Information($"MottoAPI (IsDevelopment: {app.Environment.IsDevelopment()})");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
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
