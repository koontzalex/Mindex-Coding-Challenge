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

        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request for '{compensation.Employee} {compensation.Salary}'");

            try
            {
                _compensationService.Create(compensation);
            }
            catch (NotSupportedException ex)
            {
                _logger.LogError($"Invalid employee id for compensation request '{compensation.Employee} {compensation.Salary}'");
                return UnprocessableEntity();
            }

            return CreatedAtRoute("getCompensationById", new { id = compensation.CompensationId }, compensation);
        }

        [HttpGet("{id}", Name = "getCompensationById")]
        public IActionResult GetCompensationById(String id)
        {
            _logger.LogDebug($"Received compensation get request for '{id}'");

            var compensation = _compensationService.GetById(id);

            if (compensation == null)
                return NotFound();

            return Ok(compensation);
        }

        [HttpPut("{id}")]
        public IActionResult ReplaceCompensation(String id, [FromBody]Compensation newCompensation)
        {
            _logger.LogDebug($"Recieved compensation update request for '{id}'");

            var existingCompensation = _compensationService.GetById(id);
            if (existingCompensation == null)
                return NotFound();

            try
            {
                
            _compensationService.Replace(existingCompensation, newCompensation);
            }
            catch (NotSupportedException ex)
            {
                _logger.LogError($"Invalid employee id for compensation request '{id}'");
                return UnprocessableEntity();
            }

            return Ok(newCompensation);
        }
    }
}
