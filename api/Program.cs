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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Add OpenAPI 3.0 document serving middleware
    // Available at: http://localhost:<port>/swagger/v1/swagger.json
    app.UseOpenApi();

    // Add web UIs to interact with the document
    // Available at: http://localhost:<port>/swagger
    app.UseSwaggerUi(); // UseSwaggerUI Protected by if (env.IsDevelopment())
}

app.UseHttpsRedirection();

Console.WriteLine($"MottoAPI (IsDevelopment: {app.Environment.IsDevelopment()})"); // Console message

app.Run();
