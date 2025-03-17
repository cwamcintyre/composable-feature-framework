using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using Component.Form.Application;
using Component.Search.API;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Azure.Core.Serialization;
using System.Text.Json;
using Component.Core.SafeJson;
using Component.Form.Model;

namespace TestAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = FunctionsApplication.CreateBuilder(args);

        builder.ConfigureFunctionsWebApplication();
        builder.Services.AddFormApplicationServices();
        builder.Services.AddSearchApplicationServices(new SearchApplicationServiceOptions { UseFakes = true });
        builder.Services.Configure<WorkerOptions>(options =>
        {
            options.Serializer = new JsonObjectSerializer(new JsonSerializerOptions
            {                
                Converters =
                {
                    new PolymorphicJsonConverter<PageBase>() // Custom converter for type hints
                }
            });
        });

        var config = builder.Configuration;

        // // Add in-memory configuration for missing keys or overrides
        // config.AddInMemoryCollection(new Dictionary<string, string>
        // {
        //     { "Functions:Worker:HostEndpoint", "http://localhost:7071" }  // Example function worker endpoint
        // });

        // // Optionally, add other configuration sources if necessary
        config.AddJsonFile("local.settings.json", optional: true);

        // Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
        // builder.Services
        //     .AddApplicationInsightsTelemetryWorkerService()
        //     .ConfigureFunctionsApplicationInsights();

        builder.Build().Run();
    }
}