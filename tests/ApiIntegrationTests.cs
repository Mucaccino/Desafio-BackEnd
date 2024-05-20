using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Motto.Models;
using Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

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
            .AddEnvironmentVariables() // Adicione essa linha para incluir as variáveis de ambiente
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)                
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

        var json = JsonSerializer.Serialize(new LoginModel() { Username = userType == UserType.Admin ? "admin" : "entregador", Password = "123mudar" });
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/api/Auth/login", data);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<LoginModelResponse>();
        _loggedToken = result?.Token;
        _loggedUserType = userType;
        Assert.IsTrue(_loggedToken != null && _loggedToken.Length > 0);
    }

    [TestMethod]
    [DataRow(UserType.Admin)]
    [DataRow(UserType.DeliveryDriver)]
    public async Task ApiIntegration_Verify(UserType userType)
    {
        if (string.IsNullOrEmpty(_loggedToken) || userType != _loggedUserType) {
            await ApiIntegration_Login(userType);
        }

        var client = _factory.CreateClient();

        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/Auth/verify/{userType.ToString().ToLower()}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _loggedToken?.Replace("Bearer ", ""));

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}