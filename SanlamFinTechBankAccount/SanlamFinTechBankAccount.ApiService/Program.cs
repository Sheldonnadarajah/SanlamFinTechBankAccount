using SanlamFinTechBankAccount.Application.Events;
using SanlamFinTechBankAccount.Application.Services;
using SanlamFinTechBankAccount.Application.Repositories;
using SanlamFinTechBankAccount.Infrastructure.Events;
using SanlamFinTechBankAccount.Infrastructure.Repositories;
using System.Data;
using Microsoft.Data.SqlClient;
using Amazon.SimpleNotificationService;
using Amazon.Extensions.NETCore.Setup;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Dependency Injection registrations


builder.Services.AddScoped<IBankAccountService, BankAccountService>();
builder.Services.AddScoped<IEventPublisher, SnsEventPublisher>();
builder.Services.AddScoped<IBalanceRepository, BalanceRepository>();
builder.Services.AddAWSService<IAmazonSimpleNotificationService>();
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("BankDb")));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
