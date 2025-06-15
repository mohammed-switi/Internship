namespace AnamolyDetectionService;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IMongoRepository
{

    Task InsertAsync<T>(T document);


    Task<IEnumerable<T>> GetRecentAsync<T>(int count);

    
    Task<IEnumerable<T>> GetByServerAsync<T>(string serverIdentifier);
}
