using MongoDb.Repository;
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

        public IEnumerable<Application> GetApplications()
        {
            return _repository.GetDocuments(Application.TableName);
        }
    }
}

