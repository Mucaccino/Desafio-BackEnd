using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Motto.Entities;

var builder = WebApplication.CreateBuilder(args);

// Define default URLs 
var urls = new string[] { "http://localhost:5000", "https://localhost:5001" };
builder.WebHost.UseUrls(urls);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure NSwag
builder.Services.AddOpenApiDocument(document =>
{
    document.Title = "MottoAPI";
    document.Version = "v1";
    document.Description = "API Documentation using NSwag";
});

// Configure JWT
var jwtKey = new string(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();
// Configure dbcontext and login
builder.Services.AddDbContext<ApplicationDbContext>();
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
