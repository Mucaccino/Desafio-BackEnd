using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Motto.Data.Repositories;
using Motto.Domain.Services;
using NSwag.Generation.Processors.Security;
using Serilog;
using Serilog.Exceptions;
using Motto.Data;
using Motto.Domain.Events;
using System.Text.Json.Serialization;
using Motto.Data.Repositories.Interfaces;
using Motto.Data.Enums;
using Motto.Domain.Services.Interfaces;

namespace Motto.WebApi;

/// <summary>
/// The Program class is the entry point of the application.
/// </summary>
public partial class Program {

    private static void Main(string[] args)
    {        
        var builder = WebApplication.CreateBuilder(args);

        // Configure logging
        builder.Logging.ClearProviders();
        builder.Host.UseSerilog((context, loggerConfig) =>
        {
            loggerConfig.ReadFrom.Configuration(context.Configuration)
                        .Enrich.FromLogContext()
                        .Enrich.WithExceptionDetails();
        });

        // Add services to the container
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            Log.Information($"UseNpgsql (ConnectionString: {builder.Configuration.GetConnectionString("DefaultConnection")})");
        });

        builder.Services.AddControllers()
            // Add support for JSON string enums
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); ;

        // Register repositories
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IDeliveryDriverUserRepository, DeliveryDriverUserRepository>();
        builder.Services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
        builder.Services.AddScoped<IRentalPlanRepository, RentalPlanRepository>();

        // Register services
        builder.Services.AddScoped<IAuthService, AuthService>(provider =>
        {
            return new AuthService(provider.GetRequiredService<IUserRepository>(), builder.Configuration["Jwt:Key"] ?? "", provider.GetRequiredService<ILogger<AuthService>>());
        });
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IRentalPlanService, RentalPlanService>();
        builder.Services.AddScoped<ILicenseImageService, LicenseImageService>();
        builder.Services.AddScoped<LicenseImageService>();

        builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

        // Register RabbitMQ services
        builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();
        builder.Services.AddSingleton<MotorcycleEventProducer>();

        // Configure authentication
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

        // Configure authorization
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy => policy.RequireRole(UserType.Admin.ToString()));
            options.AddPolicy("DeliveryDriver", policy => policy.RequireRole(UserType.DeliveryDriver.ToString()));
        });

        // Configure CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        // Configure Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerDocument(config =>
        {
            config.Title = "MottoAPI";
            config.Version = "v1";
            config.Description = "API Documentation using NSwag";
            config.AddSecurity("Bearer", new NSwag.OpenApiSecurityScheme
            {
                Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                Description = "JWT Authorization header using the Bearer scheme (Ex: Bearer eyJhbGciOiJIUzI1NiIsI...)",
                In = NSwag.OpenApiSecurityApiKeyLocation.Header
            });
            config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
            config.OperationProcessors.Add(new OperationSecurityScopeProcessor("Bearer"));
        });

        var app = builder.Build();

        if (!app.Environment.IsProduction())
        {
            app.UseOpenApi();
            app.UseSwaggerUi();
            app.UseReDoc(options =>
            {
                options.Path = "/redoc";
            });
        }

        // Configure CORS policies
        app.UseCors("AllowAllOrigins");

        app.UseSerilogRequestLogging();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}

public partial class Program { }
