using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/compensation")]
    public class CompensationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

/// <summary>
/// Creates a new Compensation entity, based on the payload in the body.
/// The employee id must reference a valid employee.
/// There must not be a preexisting compensation for the employee.
/// </summary>
        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request for '{compensation.Employee.EmployeeId} {compensation.Salary}'");

            try
            {
                _compensationService.Create(compensation);
            }
            catch (NotSupportedException ex)
            {
                _logger.LogError(ex, $"Invalid Compensation Create Request");
                return UnprocessableEntity(compensation);
            }

            return CreatedAtRoute("getCompensationById", new { id = compensation.CompensationId }, compensation);
        }

/// <summary>
/// Returns the compensation based on the id.
/// Returns a 404 if no compensation with that id exists.
/// </summary>
        [HttpGet("{id}", Name = "getCompensationById")]
        public IActionResult GetCompensationById(String id)
        {
            _logger.LogDebug($"Received compensation get request for '{id}'");

            var compensation = _compensationService.GetById(id);

            if (compensation == null)
                return NotFound(id);

            return Ok(compensation);
        }

/// <summary>
/// Replaces the attributes of the Compensation which has the provided id.
/// The new attributes are from the body of the request.
/// This does not change the id of the Compensation.
/// The employee id may be changed, but must still remain valid and unique for all compensations.
/// </summary>
        [HttpPut("{id}")]
        public IActionResult ReplaceCompensation(String id, [FromBody]Compensation newCompensation)
        {
            _logger.LogDebug($"Recieved compensation update request for '{id}'");

            var existingCompensation = _compensationService.GetById(id);
            if (existingCompensation == null)
                return NotFound(id);

            try
            {
                
            _compensationService.Replace(existingCompensation, newCompensation);
            }
            catch (NotSupportedException ex)
            {
                _logger.LogError(ex, $"Invalid Compensation Update Request");
                return UnprocessableEntity(newCompensation);
            }

            return Ok(newCompensation);
        }
    }
}
