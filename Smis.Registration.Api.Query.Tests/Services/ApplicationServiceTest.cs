using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Moq;
using Smis.MongoDb.Lib.Repositories;
using Smis.Registration.Api.Query.ErrorHandler;
using Smis.Registration.Api.Query.Services;
using Smis.Registration.Persistence.Lib;

namespace Smis.Registration.Api.Query.Tests.Services
{
    public class ApplicationServiceTest
	{
        private Mock<IMongoReadRepository<Application>> repo;
        private Mock<ILogger<ApplicationService>> logger = new Mock<ILogger<ApplicationService>>();
        private ApplicationService service;

        public ApplicationServiceTest()
		{
            repo = new Mock<IMongoReadRepository<Application>>();
            service = new ApplicationService(logger.Object, repo.Object);
        }

		[Fact]
		public async Task  GetApplications_shouldCallTheRepository()
		{
			repo.Setup(r => r.GetDocuments(It.IsAny<string>())).Returns(Task.FromResult(new List<Application>()));	

            await service.GetApplications();

			repo.Verify(r => r.GetDocuments(It.Is<string>(a => a.Equals(Application.TableName))));
			
		}

        [Fact]
        public async Task GetApplications_shouldReturnAListOfApplication()
        {
            var applications = new List<Application>() { CreateApplication() };
            
            repo.Setup(r => r.GetDocuments(It.IsAny<string>())).Returns(Task.FromResult(applications));

            var actual = await service.GetApplications();

            Assert.Equal(applications, actual);

        }

        [Fact]
        public async Task GetApplication_shouldReturnAnApplicationForApplicationNumber()
        {
            var application = CreateApplication();

            repo.Setup(r => r.GetDocument(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<Application?>(application));

            var actual = await service.GetApplication("123");

            Assert.Equal(application, actual);
        }

        [Fact]
        public async Task GetApplication_ShouldUseAnEqualityFilterOnApplicationNumber()
        {
            var application = CreateApplication();

            repo.Setup(r => r.GetDocument(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<Application?>(application));

            await service.GetApplication("123");

            verifyGetDocument();
        }

        [Fact]
        public async Task GetApplication_ReturnNotFoundException()
        {
            repo.Setup(r => r.GetDocument(It.IsAny<string>(), It.IsAny<FilterDefinition<Application>>()))
                .Returns(Task.FromResult<Application?>(null));

            await  Assert.ThrowsAsync<ApplicationNotFoundException>(() => service.GetApplication("123"));            
        }

        private void verifyGetDocument()
        {
            var serializerRegistry = BsonSerializer.SerializerRegistry;
            var documentSerializer = serializerRegistry.GetSerializer<Application>();
            var filter = Builders<Application>.Filter.Eq(a => a.ApplicationNumber, "123");

            repo.Verify(r => r.GetDocument(
                It.Is<string>(s => s.Equals(Application.TableName)),
                It.Is<string>(f => f.Equals("123")))
            );
        }

        private Application CreateApplication()
        {
            var address = new Address
            {
                AddressLine1 = "109",
                AddressLine2 = "England Avenue",              
                PostCode = "NE8 1QR"
            };

            return new Application()
            {
                FirstName = "Mauro",
                Surname = "Watson",
                DateOfBirthday = DateTime.Parse("2022-01-01"),
                ApplicationNumber = "123",
                Title = "Master",
                ContactAddress = address,
                PrimaryGuardianName = "Maria",
                PrimaryGuardianSurname = "Watson",
                PrimaryGuardianTelephone = "01912345678"
            };
        }
    }
}

