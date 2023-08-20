using System;
using Microsoft.AspNetCore.Mvc;
using Smis.Registration.Persistence.Lib;

namespace Smis.Registration.Api.Query.Services
{
	public interface IApplicationService
	{
        Task<Application> GetApplication(string applicationNumber);
        Task<IEnumerable<Application>> GetApplications();

    }
}

