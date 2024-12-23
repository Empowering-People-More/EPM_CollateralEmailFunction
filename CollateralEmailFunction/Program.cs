using CollateralEmailFunction;
using CollateralEmailFunction.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

var value = Environment.GetEnvironmentVariable("myKey");



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddAzureClients(azureClientFactoryBuilder =>
{
    var config = new ConfigurationBuilder()
        .SetBasePath(Environment.CurrentDirectory)
        .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
        .Build();

    azureClientFactoryBuilder.AddSecretClient(config.GetSection("KeyVault"));
});

builder.Services.AddSingleton<IKeyVaultManager, KeyVaultManager>();

builder.Services.AddSingleton(serviceProvider =>
{
    var secretManager = serviceProvider.GetRequiredService<IKeyVaultManager>();
    var logger = serviceProvider.GetRequiredService<ILogger<EncompassCredentials>>();

    // Call the asynchronous method synchronously
    var credentials = Task.Run(async () => await GetEncompassCredentials(secretManager, logger)).GetAwaiter().GetResult();

    return credentials;
});

static async Task<EncompassCredentials> GetEncompassCredentials(IKeyVaultManager secretManager, ILogger<EncompassCredentials> logger)
{
    try
    {
        return new EncompassCredentials
        {
            Username = await secretManager.GetSecretAsync("Username"),
            Password = await secretManager.GetSecretAsync("Password"),
            DisclosureDeskUsername = await secretManager.GetSecretAsync("DisclosureDeskUsername"),
            DisclosureDeskPassword = await secretManager.GetSecretAsync("DisclosureDeskPassword"),
            InstanceId = await secretManager.GetSecretAsync("InstanceId"),
            ClientId = await secretManager.GetSecretAsync("ClientId"),
            ClientSecret = await secretManager.GetSecretAsync("ClientSecret"),
            SendGridKey = await secretManager.GetSecretAsync("SendGridKey"),
        };
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error getting Encompass credentials from KeyVault");
        return null;
    }
}






builder.Build().Run();
