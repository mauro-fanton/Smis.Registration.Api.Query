using Microsoft.Extensions.Logging;
using MongoDb.Repository;
using Moq;
using Smis.Registration.Api.Query.Services;
using Smis.Registration.Persistence.Lib;

namespace Smis.Registration.Api.Query.Tests.Services
{
    public class ApplicationServiceTest
	{
        private Mock<IMongoDbRepository<Application>> repo;
        private Mock<ILogger<ApplicationService>> logger = new Mock<ILogger<ApplicationService>>();
        private ApplicationService service;

        public ApplicationServiceTest()
		{
            repo = new Mock<IMongoDbRepository<Application>>();
            service = new ApplicationService(logger.Object, repo.Object);
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

