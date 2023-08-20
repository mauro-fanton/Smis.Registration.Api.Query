using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
		public async Task Get_ShouldreturnAListOfApplication()
		{
            var applications = new List<Application>() { CreateApplication() };
            service.Setup(s => s.GetApplications()).Returns(Task.FromResult(applications.AsEnumerable()));

            var actual = await sut.Get();
            
            Assert.Equal(applications, actual.Value);
        }

        [Fact]
        public async Task Get_ShouldreturnAnError()
        {
            var applications = new List<Application>() { CreateApplication() };
            service.Setup(s => s.GetApplications()).Throws(new InvalidOperationException("Error message"));

            var actual = await sut.Get();

            var result = actual.Result as ObjectResult;

            Assert.Equal(StatusCodes.Status500InternalServerError, result?.StatusCode);
            Assert.Equal("Error message", result?.Value);
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

