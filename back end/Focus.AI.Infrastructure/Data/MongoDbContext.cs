using Focus.AI.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Focus.AI.Infrastructure.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    private readonly MongoDbSettings _settings;

    static MongoDbContext()
    {
        BsonClassMap.RegisterClassMap<ChatSession>(cm =>
        {
            cm.AutoMap();
            cm.MapIdProperty(c => c.Id)
              .SetIdGenerator(StringObjectIdGenerator.Instance)
              .SetSerializer(new StringSerializer(BsonType.ObjectId));
        });

        BsonClassMap.RegisterClassMap<ChatMessage>(cm =>
        {
            cm.AutoMap();
            cm.MapIdProperty(c => c.Id);
        });
    }

    public MongoDbContext(IOptions<MongoDbSettings> options)
    {
        _settings = options.Value;
        var client = new MongoClient(_settings.ConnectionString);
        _database = client.GetDatabase(_settings.DatabaseName);
    }

    public IMongoCollection<ChatSession> ChatSessions => _database.GetCollection<ChatSession>(_settings.ChatSessionsCollection);
}
