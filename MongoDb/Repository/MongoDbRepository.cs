using MongoDb.Connection;
using MongoDB.Driver;

namespace MongoDb.Repository
{
    public class MongoDbRepository<TDocument> : IMongoDbRepository<TDocument> where TDocument: class
    {
        private readonly IMongoDbConnection _connection;

        public MongoDbRepository(IMongoDbConnection connection)
        {
            _connection = connection;
        }

		public async Task<List<TDocument>> GetDocuments(string collectionName)
		{
			var db = _connection.GetCollection<TDocument>(collectionName);

            return await db.Aggregate().ToListAsync();
        }

        public async Task<TDocument?> GetDocument(string collectionName, FilterDefinition<TDocument> filter)
        {
            var db = _connection.GetCollection<TDocument>(collectionName);

            return await db.Find(filter).SingleOrDefaultAsync();
        }
    }
}

