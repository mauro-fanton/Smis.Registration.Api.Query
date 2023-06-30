namespace MongoDb.Repository
{
    public interface IMongoDbRepository<TDocument> where TDocument : class
    {
        List<TDocument> GetDocuments(string collectionName);

    }
}

