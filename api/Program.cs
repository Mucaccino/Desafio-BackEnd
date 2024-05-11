using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Motto.Entities;
using Motto.Models;
using NSwag.Generation.Processors.Security;

var builder = WebApplication.CreateBuilder(args);

// Define default URLs 

var urls = new string[] { "http://localhost:5000", "https://localhost:5001" };
builder.WebHost.UseUrls(urls);

// Add services to the container

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

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

// Configure dbcontext and login
builder.Services.AddSingleton(jwtKey);

var app = builder.Build();

Console.WriteLine($"MottoAPI (IsDevelopment: {app.Environment.IsDevelopment()})");

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

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
