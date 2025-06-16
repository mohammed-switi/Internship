using MongoDB.Driver;

namespace AnomalyDetectionService.Repositories;

public class MongoRepository : IMongoRepository
{
    private readonly IMongoDatabase _database;

    public MongoRepository(IMongoClient mongoClient, string databaseName)
    {
        ArgumentNullException.ThrowIfNull(mongoClient);
        if (string.IsNullOrWhiteSpace(databaseName)) throw new ArgumentException("Database name must be provided.");

        _database = mongoClient.GetDatabase(databaseName);
    }

    private IMongoCollection<T> GetCollection<T>()
    {
        var collectionName = typeof(T).Name;
        return _database.GetCollection<T>(collectionName);
    }

    public async Task InsertAsync<T>(T document)
    {
        try
        {
            var collection = GetCollection<T>();
            await collection.InsertOneAsync(document);
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"[MongoRepository] Insert failed: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<T>> GetRecentAsync<T>(string serverIdentifier,int count)
    {
        try
        {
            var collection = GetCollection<T>();
            var filter = Builders<T>.Filter.Eq("serverIdentifier", serverIdentifier);
            var sortDefinition = Builders<T>.Sort.Descending("timestamp");

            return await collection
                .Find(filter)
                .Sort(sortDefinition)
                .Limit(count)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"[MongoRepository] GetRecentAsync failed: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<T>> GetByServerAsync<T>(string serverIdentifier)
    {
        try
        {
            var collection = GetCollection<T>();
            var filter = Builders<T>.Filter.Eq("serverIdentifier", serverIdentifier);
            return await collection.Find(filter).ToListAsync();
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"[MongoRepository] GetByServerAsync failed: {ex.Message}");
            throw;
        }
    }
}