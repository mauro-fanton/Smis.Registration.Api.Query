using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Smis.Registration.Api.Query.It.Test.Fixture;

namespace Smis.Registration.Api.Query.It.Test;

public class RegistrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;


    public RegistrationTests(CustomWebApplicationFactory<Program> factory)
    {
        factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/smis/register/");

        _client = factory.CreateClient();
    }

    [Fact]
    public void Test1()
    {

    }
}

