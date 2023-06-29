using System;
using MongoDb.Repository;
using Smis.Registration.Persistence.Lib;

namespace Smis.Registration.Api.Query.Services
{
	public class ApplicationService : IApplicationService
	{
        private readonly IMongoDbRepository<Application> _repository;

        public ApplicationService(IMongoDbRepository<Application> repository)
		{
            _repository = repository;
        }

        public IEnumerable<Application> GetApplications()
        {
            return _repository.GetDocuments(Application.TableName);
        }
    }
}

