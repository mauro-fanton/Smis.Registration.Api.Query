using Microsoft.Extensions.Logging;
using Moq;
using Smis.Registration.Api.Query.Controllers;
using Smis.Registration.Api.Query.Services;
using Smis.Registration.Persistence.Lib;


namespace Smis.Registration.Api.Query.Tests.Controllers
{
    public class ApplicationControllerTests
	{
        private Mock<ILogger<ApplicationController>> logger;
        private Mock<IApplicationService> service;
        private ApplicationController sut;

        public ApplicationControllerTests()
		{
            logger = new Mock<ILogger<ApplicationController>>();
            service = new Mock<IApplicationService>();
            sut = new ApplicationController(logger.Object, service.Object);
        }

		[Fact]
		public void Get_ShouldreturnOK()
		{
            var applications = new List<Application>() { CreateApplication() };
            service.Setup(s => s.GetApplications()).Returns(applications);

            Assert.Equal(applications, sut.Get());

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

