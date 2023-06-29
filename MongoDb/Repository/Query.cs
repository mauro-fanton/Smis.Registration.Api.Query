using System;
using MongoDB.Driver;

namespace MongoDb.Repository
{
	public class Query<TCollection> : IQuery<TCollection>
	{
		public Query()
		{
		}

        public IQueryable<TCollection> Select(IMongoCollection<TCollection> db)
        {
            return from t in db.AsQueryable() select t;
        }
    }
}

