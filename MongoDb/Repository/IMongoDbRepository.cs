using MongoDB.Driver;

namespace MongoDb.Repository
{
    public interface IMongoDbRepository<TDocument> where TDocument : class
    {
        Task<List<TDocument>> GetDocuments(string collectionName);
        Task<TDocument?> GetDocument(string collectionName, FilterDefinition<TDocument> filter);

    }
}

