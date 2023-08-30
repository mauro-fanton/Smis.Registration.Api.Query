using Microsoft.AspNetCore.Mvc;
using Smis.Registration.Api.Query.ErrorHandler;
using Smis.Registration.Api.Query.Services;
using Smis.Registration.Persistence.Lib;



namespace Smis.Registration.Api.Query.Controllers;

[Produces("application/json")]
[ApiController]
[Route("api/smis")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class ApplicationController : ControllerBase
{
    private readonly ILogger<ApplicationController> _logger;
    private readonly IApplicationService _service;

    public ApplicationController(ILogger<ApplicationController> logger, IApplicationService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <summary>
    /// Get all resgitered applicationa
    /// </summary>
    /// <returns> A list of Application</returns>
    /// <response code="200"> Returns a list of Application </response>
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    [HttpGet]
    [Route("applications")]
    public async Task<ActionResult<List<Application>>> Get()
    {
        try
        {
            var applications = await _service.GetApplications();
            return applications.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retriving all documents");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Get a resgitered application with application number
    /// </summary>
    /// <returns> A Registered Application</returns>
    /// <response code="200"> Returns a registered application </response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    [HttpGet]
    [Route("application/{applicationNumber}")]
    public async Task<ActionResult<Application>> Get(string applicationNumber)
    {
        try
        {
            return await _service.GetApplication(applicationNumber); 
        }
        catch(ApplicationNotFoundException ex)
        {
            _logger.LogError($"Application {applicationNumber} not found", ex);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retriving application: {applicationNumber}");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

    }
}

