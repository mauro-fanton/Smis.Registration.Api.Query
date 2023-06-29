using MongoDb.Connection;
using MongoDB.Driver;

namespace MongoDb.Repository
{
    public class MongoDbRepository<TDocument> : IMongoDbRepository<TDocument> where TDocument: class
    {
        private readonly IMongoDbConnection connection;
        private readonly IQuery<TDocument> query;

        public MongoDbRepository(IMongoDbConnection Connection, IQuery<TDocument> query)
		{
            connection = Connection;
            this.query = query;
        }

		public List<TDocument> GetDocuments(string collectionName)
		{
			var db = connection.GetCollection<TDocument>(collectionName);

			return query.Select(db).ToList();
        }
	}
}

