using MongoDb.Repository;
using Moq;
using Smis.Registration.Api.Query.Services;
using Smis.Registration.Persistence.Lib;

namespace Smis.Registration.Api.Query.Tests.Services
{
    public class ApplicationServiceTest
	{
        Mock<IMongoDbRepository<Application>> repo;
        ApplicationService service;

        public ApplicationServiceTest()
		{
            repo = new Mock<IMongoDbRepository<Application>>();
            service = new ApplicationService(repo.Object);
        }

		[Fact]
		public void GetApplication_shouldCallTheRepository()
		{
			repo.Setup(r => r.GetDocuments(It.IsAny<string>())).Returns(new List<Application>());	

            service.GetApplications();

			repo.Verify(r => r.GetDocuments(Application.TableName));
			
		}

        [Fact]
        public void GetApplication_shouldReturnAListOfApplication()
        {
            var applications = new List<Application>() { CreateApplication() };
            
            repo.Setup(r => r.GetDocuments(It.IsAny<string>())).Returns(applications);

            Assert.Equal(applications, service.GetApplications());

        }

      

        private Application CreateApplication()
        {
            var address = new Address(
                "109",
                "England Avenue",
                string.Empty,
                string.Empty,
                "NE8 1QR"
            );

            return new Application(
                "Mauro",
                "Watson",
                DateTime.Parse("2022-01-01"),
                "123",
                "Master",
                address,
                "Maria",
                "Watson",
                "01912345678"
            );
        }
    }
}

