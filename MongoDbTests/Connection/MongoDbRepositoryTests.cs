using MongoDb.Connection;
using MongoDb.Repository;
using MongoDB.Driver;
using MongoDbTests.Models;
using Moq;

namespace MongoDbTests.Connection
{
    public class MongoDbRepositoryTests { 


        private Mock<IMongoCollection<TestModel>> collection;
		private Mock<IMongoDbConnection> connection;
		private Mock<IQuery<TestModel>> query;
		private MongoDbRepository<TestModel> repo;

	
		public MongoDbRepositoryTests()
		{
			collection = new Mock<IMongoCollection<TestModel>>();
			connection = new Mock<IMongoDbConnection>();
			query = new Mock<IQuery<TestModel>>();
            repo = new MongoDbRepository<TestModel>(connection.Object, query.Object);
        }

		[Fact]
		public void FindAll_shouldReturnAllDocuments()
		{
			var collectionItem = new List<TestModel>() { new TestModel("mauro", DateTime.Parse("2015-06-12"), "electrician")};

			query.Setup(q => q.Select(It.IsAny<IMongoCollection<TestModel>>())).Returns(collectionItem.AsQueryable());
			connection.Setup(c => c.GetCollection<TestModel>(It.IsAny<string>())).Returns(collection.Object);

			var res = repo.GetDocuments("collectionName");

			connection.Verify(c => c.GetCollection<TestModel>("collectionName"));


		}
	}
}

