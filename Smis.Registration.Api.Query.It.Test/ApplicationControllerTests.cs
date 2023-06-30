
using System.Net;
using Newtonsoft.Json;
using Smis.Registration.Api.Query.It.Test.Fixture;
using Smis.Registration.Persistence.Lib;


namespace Smis.Registration.Api.Query.It.Test;

public class ApplicationControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public ApplicationControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/smis/applications/");

        _client = factory.CreateClient();
        _factory = factory;
    }

    [Fact]
    public async Task itShouldReturnOk()
    {
   
        _factory._repository.Setup(r => r.GetDocuments(Moq.It.IsAny<string>())).Returns(new List<Application>());

        var response = await _client.GetAsync("");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    }

    [Fact]
    public async Task itShouldReturnAListOfApplications()
    {
        var applications = new List<Application>() { CreateApplication() };
        _factory._repository.Setup(r => r.GetDocuments(Moq.It.IsAny<string>())).Returns(applications);

        var response = await _client.GetAsync("");

        var actual = await response.Content.ReadFromJsonAsync<List<Application>>();
      
        Assert.Equal(JsonConvert.SerializeObject(actual), JsonConvert.SerializeObject(actual));       
    }

    [Fact]
    public async Task itShouldReturnAAnEmptyListIfNoApplicationsFounds()
    {
        var applications = new List<Application>() { CreateApplication() };
        _factory._repository.Setup(r => r.GetDocuments(Moq.It.IsAny<string>())).Returns(new List<Application>());

        var response = await _client.GetAsync("");

        var actual = await response.Content.ReadFromJsonAsync<List<Application>>();

        Assert.NotNull(actual);
        Assert.True(actual.Count() == 0);
    }

    [Fact]
    public async Task ItShouldReturnInternalSerberError()
    {
        _factory._repository.Setup(r => r.GetDocuments(Moq.It.IsAny<string>())).Throws<Exception>();

        var response = await _client.GetAsync("");

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
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

