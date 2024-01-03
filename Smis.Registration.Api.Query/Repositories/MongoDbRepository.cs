
using MongoDB.Driver;
using Smis.MongoDb.Lib.Connection;
using Smis.MongoDb.Lib.Repositories;

namespace Smis.Registration.Api.Query.Repositories
{
    public class MongoDbRepository<TDocument> : IMongoReadRepository<TDocument> where TDocument: class
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

        public async Task<TDocument?> GetDocument(string collectionName, string applicationNumber)
        {
            var db = _connection.GetCollection<TDocument>(collectionName);

            var filterByApplicationNumber = Builders<TDocument>.Filter.Eq("applicationNumber", applicationNumber);
            return await GetDocument(collectionName, filterByApplicationNumber);
        }
    }
}

