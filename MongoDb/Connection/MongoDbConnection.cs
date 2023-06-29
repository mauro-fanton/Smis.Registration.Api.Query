using System;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using static System.Net.Mime.MediaTypeNames;

namespace MongoDb.Connection
{
	public class MongoDBConnection: IMongoDbConnection
	{
		private readonly MongoClient client;
		private readonly string database;

        public MongoDBConnection(IConfiguration configuration)
		{
            client = new MongoClient(configuration.GetConnectionString("mongodb"));
			database = configuration.GetValue<string>("ConnectionStrings:database")
				?? throw new InvalidDataException("Could Not find database name");
		
        }

		public IMongoCollection<Document> GetCollection<Document>(string collectioName)
		{
			return client.GetDatabase("").GetCollection<Document>(collectioName);
        }
    }	
}

