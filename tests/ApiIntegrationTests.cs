using System.Text;
using System.Text.Json;
using Xunit;
using Microsoft.AspNetCore.Http;
using Motto.Models;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace Motto.Tests;

[TestClass]
public class ApiIntegrationTests : IClassFixture<Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<Program>>
{
    private readonly Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<Program> _factory;
    private UserType _loggedUserType;
    private string? _loggedToken;
    
    public ApiIntegrationTests()    
    {
        _factory = new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<Program>()
        .WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // services.AddSingleton<IHelloService, MockHelloService>();
            });
        });
    }

    [TestMethod]
    [DataRow(UserType.Admin)]
    [DataRow(UserType.DeliveryDriver)]
    public async Task AuthTest_Login(UserType userType)
    {
        var client = _factory.CreateClient();

        var json = JsonSerializer.Serialize(new LoginModel() { Username = userType == UserType.Admin ? "admin" : "entregador", Password = "123mudar" });
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/api/Auth/login", data);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<LoginModelResponse>();

        _loggedToken = result?.Token;
        _loggedUserType = userType;

        Xunit.Assert.Contains("Bearer ", _loggedToken);
    }

    [TestMethod]
    [DataRow(UserType.Admin)]
    [DataRow(UserType.DeliveryDriver)]
    public async Task AuthTest_Login_Verify(UserType userType)
    {
        if (string.IsNullOrEmpty(_loggedToken) || userType != _loggedUserType) {
            await AuthTest_Login(userType);
        }

        var client = _factory.CreateClient();

        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/Auth/verify/{userType.ToString().ToLower()}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _loggedToken?.Replace("Bearer ", ""));

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    // protected virtual void ConfigureWebHost(IWebHostBuilder builder)
    // {
    //     Environment.SetEnvironmentVariable("CacheSettings:UseCache", "false");

    //     _ = builder.ConfigureTestServices(services =>
    //     {
    //         services.AddScoped<Interface, ImplementationFake>();
    //     });
    // }

    // [Theory]
    // [InlineData("/api/Auth/login")]
    // // Adicione mais endpoints para testar aqui
    // public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    // {
    //     // Arrange
    //     var client = _factory.CreateClient();

    //     // Act
    //     var response = await client.GetAsync(url);

    //     // Assert
    //     response.EnsureSuccessStatusCode(); // Verifica se o código de status da resposta é de sucesso (200-299)
    //     Assert.Equal("application/json", response.Content.Headers.ContentType.ToString()); // Verifica se o tipo de conteúdo retornado é JSON
    // }

    // [Theory]
    // [InlineData("/api/endpoint3")]
    // // Adicione mais endpoints para testar aqui
    // public async Task Post_EndpointsReturnSuccessAndCorrectContentType(string url)
    // {
    //     // Arrange
    //     var client = _factory.CreateClient();
    //     var requestBody = new
    //     {
    //         // Seu objeto de request aqui
    //     };

    //     var json = JsonSerializer.Serialize(requestBody);
    //     var data = new StringContent(json, Encoding.UTF8, "application/json");

    //     // Act
    //     var response = await client.PostAsync(url, data);

    //     // Assert
    //     response.EnsureSuccessStatusCode(); // Verifica se o código de status da resposta é de sucesso (200-299)
    //     Assert.Equal("application/json", response.Content.Headers.ContentType.ToString()); // Verifica se o tipo de conteúdo retornado é JSON
    // }
}
//[TestMethod]