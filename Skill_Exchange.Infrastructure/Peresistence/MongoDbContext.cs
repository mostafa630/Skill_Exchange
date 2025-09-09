using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Infrastructure.Configurations;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<Message> Messages => _database.GetCollection<Message>("messages");
}