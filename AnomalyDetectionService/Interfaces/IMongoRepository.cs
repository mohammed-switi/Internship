namespace AnomalyDetectionService;

public interface IMongoRepository
{

    Task InsertAsync<T>(T document);


    Task<IEnumerable<T>> GetRecentAsync<T>(string serverIdentifier, int count);

    
    Task<IEnumerable<T>> GetByServerAsync<T>(string serverIdentifier);
}