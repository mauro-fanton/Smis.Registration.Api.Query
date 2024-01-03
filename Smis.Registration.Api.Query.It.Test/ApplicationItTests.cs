
using System.Net;
using MongoDB.Driver;
using Newtonsoft.Json;
using Smis.Registration.Api.Query.Integration.Test.Fixture;
using Smis.Registration.Persistence.Lib;
using Moq;


namespace Smis.Registration.Api.Query.Integration.Test;

public class ApplicationItTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private const string GET_ALL_APP_URL = "/api/smis/applications";
    private const string GET_APPLIACTION = "/api/smis/application";

    public ApplicationItTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }

    [Fact]
    public async Task itShouldReturnOkWhenGettingAllApplication()
    {   
        _factory._repository.Setup(r => r.GetDocuments(Moq.It.IsAny<string>())).Returns(Task.FromResult(new List<Application>()));

        var response = await _client.GetAsync(GET_ALL_APP_URL);

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task itShouldReturnAListOfApplications()
    {
        var applications = new List<Application>() { CreateApplication() };
        _factory._repository.Setup(r => r.GetDocuments(Moq.It.IsAny<string>())).Returns(Task.FromResult(applications));

        var response = await _client.GetAsync(GET_ALL_APP_URL);

        var actual = await response.Content.ReadFromJsonAsync<List<Application>>();
      
        Assert.Equal(JsonConvert.SerializeObject(applications), JsonConvert.SerializeObject(actual));       
    }

    [Fact]
    public async Task itShouldReturnAnEmptyListIfNoApplicationsFounds()
    {
        _factory._repository.Setup(r => r.GetDocuments(Moq.It.IsAny<string>())).Returns(Task.FromResult(new List<Application>()));

        var response = await _client.GetAsync(GET_ALL_APP_URL);

        var actual = await response.Content.ReadFromJsonAsync<List<Application>>();

        Assert.NotNull(actual);
        Assert.True(actual.Count() == 0);
    }

    [Fact]
    public async Task ItShouldReturnInternalServerErrorWhenGettingAllApplication()
    {
        _factory._repository.Setup(r => r.GetDocuments(Moq.It.IsAny<string>())).Throws<Exception>();

        var response = await _client.GetAsync(GET_ALL_APP_URL);

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task ItSHouldRetrieveOneApplicationWithApplicationNumber()
    {
        var applications = CreateApplication();
        _factory._repository.Setup(r => r.GetDocument(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult<Application?>(applications));

        var response = await _client.GetAsync($"{GET_APPLIACTION}/{Guid.NewGuid}");

        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<Application>();

        Assert.Equal(JsonConvert.SerializeObject(applications), JsonConvert.SerializeObject(body));
    }

    [Fact]
    public async Task ItShouldReturnAnErrorWhenApplicationNotFound()
    {
        _factory._repository.Setup(r => r.GetDocument(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult<Application?>(null));

        var response = await _client.GetAsync($"{GET_APPLIACTION}/{Guid.NewGuid}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ItShouldReturnAnError()
    { 
        _factory._repository.Setup(r => r.GetDocument(It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();

        var response = await _client.GetAsync($"{GET_APPLIACTION}/{Guid.NewGuid}");

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

