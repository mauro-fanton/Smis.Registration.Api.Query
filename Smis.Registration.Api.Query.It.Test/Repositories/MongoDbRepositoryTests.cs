using System.Text.Json;
using Smis.MongoDb.Lib.Repositories;
using Smis.MongoDb.Test.Lib.Fixture;
using Smis.Registration.Api.Query.Repositories;
using Smis.Registration.Persistence.Lib;

namespace Smis.Registration.Api.Query.Integration.Test.Repositories;



public class MongoDbRepositoryTests: MongoDbFixture<Application>, IDisposable
{
    private IMongoReadRepository<Application> repository;
    private DateTime lastUpdated = new DateTime(2021, 07, 20).ToUniversalTime();


    public MongoDbRepositoryTests()
    {

        repository = new MongoDbRepository<Application>(Connection());
    }

    public override Dictionary<string, string> Configure()
    {
        return new Dictionary<string, string>()
            {
                {"database", "smis_registration"},
                {"collectionName", Application.TableName }
            };
    }

    [Fact]
    public async Task GetDocument_ShouldRetieveAnApplication()
    {
        
        var app = CreateApplication(Guid.NewGuid().ToString());
        Insert(app);

        var result = await repository.GetDocument(Application.TableName, app.ApplicationNumber);

        app.LastUpdated = result.LastUpdated;
        Assert.Equal(JsonSerializer.Serialize(app), JsonSerializer.Serialize(result));
       
    }

    [Fact]
    public async Task GetDocument_ShouldReturnNullIfNoApplicationFound()
    {

        var app = CreateApplication(Guid.NewGuid().ToString());
        Insert(app);

        var result = await repository.GetDocument(Application.TableName, "123");

        Assert.Null(result);       
    }

    [Fact]
    public async Task GetDocuments_ShouldRetrieveAllApplications()
    {       
        var app1 = CreateApplication(Guid.NewGuid().ToString());
        var app2 = CreateApplication(Guid.NewGuid().ToString());
        
        Insert(app1);
        Insert(app2);

        
        var result = await repository.GetDocuments(Application.TableName);

        
        Assert.Equal(2, result.Count);
        Assert.Equal(app1.ApplicationNumber, result[0].ApplicationNumber);
        Assert.Equal(app2.ApplicationNumber, result[1].ApplicationNumber);
    }

    [Fact]
    public async Task GetDocuments_ShouldReturnAnEmptyList()
    {        
        var result = await repository.GetDocuments(Application.TableName);

        Assert.Empty(result);        
    }

    private Application CreateApplication(string id)
    {
        var address = new Address
        {
            AddressLine1 = "56 Park Avenue",
            AddressLine2 = "NewCastle",
            PostCode = "UY6 7UG"
        };

        return new Application
        {
            ApplicationNumber = id,
            FirstName = "Gino",
            Surname = "Conti",
            DateOfBirthday = new DateTime(1980, 1, 1, 10, 0, 0).ToUniversalTime(),
            Title = "Mr",
            PrimaryGuardianName = "Andrea",
            PrimaryGuardianSurname = "Zico",
            PrimaryGuardianTelephone = "76567876345",
            ContactAddress = address
        };
    }

    public void Dispose()
    {
        Reset();
    }
}
