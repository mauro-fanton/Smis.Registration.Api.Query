using System;
using Smis.Registration.Persistence.Lib;

namespace Smis.Registration.Api.Query.Services
{
	public interface IApplicationService
	{
        IEnumerable<Application> GetApplications();

    }
}

