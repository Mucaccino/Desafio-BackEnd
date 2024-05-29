using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using Motto.DTOs;
using Serilog;
using Microsoft.Extensions.Configuration;
using Motto.Enums;

namespace Motto.Tests;

[TestClass]
public class ApiIntegrationTests : Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<Program>
{
    private readonly Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<Program> _factory;
    private UserType _loggedUserType;
    private string? _loggedToken;
    
    public ApiIntegrationTests()    
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)             
            .AddEnvironmentVariables() // Adicione essa linha para incluir as variáveis de ambiente   
            .Build();

        _factory = new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<Program>()
        .WithWebHostBuilder(builder =>
        { 
            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddConfiguration(configuration);
            });

            builder.ConfigureServices(services =>
            {
                //
            });
        });
    }

    [TestMethod]
    [DataRow(UserType.Admin)]
    [DataRow(UserType.DeliveryDriver)]
    public async Task ApiIntegration_Login(UserType userType)
    {
        var client = _factory.CreateClient();

        Log.Information($"BaseAddress: {client.BaseAddress}");

        var json = JsonSerializer.Serialize(new LoginRequest() { Username = userType == UserType.Admin ? "admin" : "entregador", Password = "123mudar" });
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/api/Auth/login", data);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        _loggedToken = result?.AccessToken;
        _loggedUserType = userType;
        Assert.IsTrue(_loggedToken != null && _loggedToken.Length > 0);
    }

}