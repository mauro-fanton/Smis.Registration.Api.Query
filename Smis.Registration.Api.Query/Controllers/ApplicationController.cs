using Microsoft.AspNetCore.Mvc;
using Smis.Registration.Api.Query.Services;
using Smis.Registration.Persistence.Lib;

namespace Smis.Registration.Api.Query.Controllers;

[ApiController]
[Route("api/smis")]
public class ApplicationController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<ApplicationController> _logger;
    private readonly IApplicationService _service;

    public ApplicationController(ILogger<ApplicationController> logger, IApplicationService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet]
    [Route("applications")]
    public IEnumerable<Application> Get()
    {
        return _service.GetApplications();
    }
}

