using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;
using System.Collections.Generic;
using System;

namespace CodeChallenge.Services
{
    public interface IReportingStructureService
    {
        public ReportingStructure GenerateReportingStructure(string employeeId);
    }
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeRepository _EmployeeRepository;
        private readonly ILogger<IReportingStructureService> _logger;

        public ReportingStructureService(ILogger<ReportingStructureService> logger, IEmployeeRepository EmployeeRepository)
        {
            _EmployeeRepository = EmployeeRepository;
            _logger = logger;
        }

/// <summary>
/// Returns the ReportingStructure entity, generated on request.
/// If no employee exists, then returns null.
/// </summary>
        public ReportingStructure GenerateReportingStructure(string employeeId)
        {

            Employee employee = _EmployeeRepository.GetById(employeeId);

            if(employee == null)
                return null;

            int totalReportCounts = GetReportingCount(employee, new Dictionary<String, int>());

            ReportingStructure report = new ReportingStructure
            {
                Employee = employee,
                NumberOfReports = totalReportCounts,
            };

            return report;
        }

        

/// <summary>
/// Calculates the number of reports beneath the Employee employee.
/// The Dictionary reports acts as a cache for repeated lookups.
/// </summary>
        private int GetReportingCount(Employee employee, Dictionary<String, int> reports)
        {
            if(reports.ContainsKey(employee.EmployeeId))
            {
                return reports[employee.EmployeeId];
            }
            else if(employee.DirectReports == null || employee.DirectReports.Count == 0)
            {
                reports.Add(employee.EmployeeId, 0);
                return 0;
            }
            else
            {
                int totalReports = employee.DirectReports.Count;
                foreach(Employee directReport in employee.DirectReports)
                {
                    totalReports+= GetReportingCount(directReport, reports);
                }
                reports.Add(employee.EmployeeId, totalReports);
                return totalReports;
            }
        }
    }
}
