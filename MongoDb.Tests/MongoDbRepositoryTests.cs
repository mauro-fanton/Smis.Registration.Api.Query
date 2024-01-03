using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using MongoDb.Repository;
using MongoDB.Driver;
using Smis.MongoDb.Lib.Connection;
using Smis.MongoDb.Lib.Repositories;
using Smis.MongoDb.Test.Lib.Fixture;
using Smis.Registration.Persistence.Lib;

namespace MongoDb.Tests;


public class MongoRipositoryTest : MongoDbFixture<Application>
{
    public override IMongoReadRepository<Application> CreateRepository(IMongoDbConnection connection)
    {
        return new MongoDbRepository<Application>(connection);
    }
}


public class MongoDbRepositoryTests: IClassFixture<MongoRipositoryTest>, IDisposable
{
    private readonly MongoDbFixture<Application> fixture;
    private IMongoReadRepository<Application> repository;
    private DateTime lastUpdated = new DateTime(2021, 07, 20).ToUniversalTime();


    public MongoDbRepositoryTests(MongoRipositoryTest fixture)
    {
        this.fixture = fixture;
        repository = fixture.CreateReadRepositoryFor(Application.TableName, "smis_registration");       
    }

   
    [Fact]
    public async Task Save_ShouldSaveTheApplication()
    {
        var id = Guid.NewGuid().ToString();

        await repository.Save(CreateApplication(id));

        var insertedRecord = fixture.FindAll();
        Assert.Single(insertedRecord);
        Assert.Equal(insertedRecord[0].ApplicationNumber, id);
        Assert.NotNull(insertedRecord[0].Id);
    }

    [Fact]
    public async Task Save_ShouldReplaceTheApplicationIfAlreadyExist()
    {
        var applicationNumber = Guid.NewGuid().ToString();
        var currentApp = CreateApplication(applicationNumber);
        currentApp.LastUpdated = lastUpdated;
        fixture.Insert(currentApp);

        currentApp.FirstName = "any name";

        await repository.Save(currentApp);

        var insertedRecord = fixture.FindAll();
        Assert.Single(insertedRecord);


        Assert.Equal(JsonSerializer.Serialize(currentApp), JsonSerializer.Serialize(insertedRecord[0]));
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
        fixture.Reset();
    }
}
