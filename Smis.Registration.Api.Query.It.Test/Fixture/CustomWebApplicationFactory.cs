using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Smis.MongoDb.Lib.Repositories;
using Smis.Registration.Persistence.Lib;

namespace Smis.Registration.Api.Query.Integration.Test.Fixture
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where
        TProgram : class
    {
        public Mock<IMongoReadRepository<Application>> _repository = new Mock<IMongoReadRepository<Application>>();

        public CustomWebApplicationFactory()
        {
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(_repository.Object);
            });
        }
    }
}

