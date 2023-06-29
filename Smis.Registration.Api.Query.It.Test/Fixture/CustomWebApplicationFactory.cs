using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MongoDb.Repository;
using Moq;
using Smis.Registration.Persistence.Lib;

namespace Smis.Registration.Api.Query.It.Test.Fixture
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where
        TProgram : class
    {
        public Mock<IMongoDbRepository<Application>> _repository = new Mock<IMongoDbRepository<Application>>();

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

