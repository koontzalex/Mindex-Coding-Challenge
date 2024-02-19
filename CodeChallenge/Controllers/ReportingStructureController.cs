using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/reporting_structure")]
    public class ReportingStructureController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IReportingStructureService _ReportingStructureService;

        public ReportingStructureController(ILogger<ReportingStructureController> logger, IReportingStructureService ReportingStructureService)
        {
            _logger = logger;
            _ReportingStructureService = ReportingStructureService;
        }

/// <summary>
/// Returns the ReportingStructure entity.
/// All of the DirectReports are full Employee entities, showing each Employee's DirectReports, Ad Infinitum.
/// </summary>
        [HttpGet("show/{employeeId}", Name = "showReportingStructure")]
        public IActionResult ShowReportingStructure(String employeeId)
        {
            _logger.LogDebug($"Received showReportingStructure request for '{employeeId}'");

            var filledReportingStructure = _ReportingStructureService.GenerateReportingStructure(employeeId);
            if(filledReportingStructure == null)
                return NotFound(employeeId);

            return Ok(filledReportingStructure);
        }
    }
}
