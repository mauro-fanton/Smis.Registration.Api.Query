using System.Net;
using MongoDb.Repository;
using MongoDB.Driver;
using Smis.Registration.Api.Query.ErrorHandler;
using Smis.Registration.Persistence.Lib;


namespace Smis.Registration.Api.Query.Services
{
    public class ApplicationService : IApplicationService
	{
        private readonly ILogger<ApplicationService> _logger;
        private readonly IMongoDbRepository<Application> _repository;

        public ApplicationService(ILogger<ApplicationService> logger, IMongoDbRepository<Application> repository)
		{
            _logger = logger;
            _repository = repository;
        }

        public async Task<IEnumerable<Application>> GetApplications()
        {
            return await _repository.GetDocuments(Application.TableName);
        }

        public async Task<Application> GetApplication(string applicationNumber)
        {
            Application? application = await _repository.GetDocument(Application.TableName, Builders<Application>.Filter.Eq(e => e.ApplicationNumber, applicationNumber));

            if (application is null)
            {
                ThrowNotFoundException(applicationNumber);
            }
            return application!;
        }

        private void ThrowNotFoundException(string applicationNumber)
        {            
            _logger.LogError($"Application {applicationNumber} could not be found");
            throw new ApplicationNotFoundException($"Application {applicationNumber} could not be found.");
        }
    }
}

