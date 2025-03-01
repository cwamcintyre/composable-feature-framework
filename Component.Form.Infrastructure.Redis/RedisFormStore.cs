using Component.Form.Application.UseCase.GetForm.Infrastructure;
using Component.Form.Model;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Component.Form.Infrastructure.Redis;

public class RedisFormStore : IFormStore
{
    private readonly IDatabase _database;

    public RedisFormStore(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Redis");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Redis connection string is missing from configuration.");
        }
        var connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
        _database = connectionMultiplexer.GetDatabase();
    }

    public async Task<FormModel> GetFormAsync(string id)
    {
        var formJson = await _database.StringGetAsync(id);
        if (formJson.IsNullOrEmpty)
        {
            throw new KeyNotFoundException($"Form with id {id} not found in Redis.");
        }

        var jsonString = formJson.ToString();
        
        var form = JsonConvert.DeserializeObject<FormModel>(jsonString);
        if (form == null)
        {
            throw new InvalidOperationException($"Deserialization of form with id {id} returned null.");
        }

        return form;
    }
}
