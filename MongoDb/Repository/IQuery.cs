using MongoDB.Driver;

namespace MongoDb.Repository
{
    public interface IQuery<TCollection>
    {
        IQueryable<TCollection> Select(IMongoCollection<TCollection> db);
    }
}