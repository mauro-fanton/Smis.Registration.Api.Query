using MongoDB.Driver;

namespace MongoDb.Connection
{
    public interface IMongoDbConnection
    {
        IMongoCollection<Document> GetCollection<Document>(string collectioName);
    }
}