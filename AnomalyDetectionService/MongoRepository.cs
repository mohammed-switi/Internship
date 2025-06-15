using MongoDB.Driver;

namespace AnomalyDetectionService;

public class MongoRepository : IMongoRepository
{
    private readonly IMongoDatabase _database;

    public MongoRepository(IMongoClient mongoClient, string databaseName)
    {
        if (mongoClient == null) throw new ArgumentNullException(nameof(mongoClient));
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
            Console.Error.WriteLine($"[MongoRepository] Insert failed: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<T>> GetRecentAsync<T>(int count)
    {
        try
        {
            var collection = GetCollection<T>();
            var sortDefinition = Builders<T>.Sort.Descending("timestamp");

            return await collection
                .Find(FilterDefinition<T>.Empty)
                .Sort(sortDefinition)
                .Limit(count)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[MongoRepository] GetRecentAsync failed: {ex.Message}");
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
            Console.Error.WriteLine($"[MongoRepository] GetByServerAsync failed: {ex.Message}");
            throw;
        }
    }
}