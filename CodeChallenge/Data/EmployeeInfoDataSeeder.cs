using CodeChallenge.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Data
{
    public class EmployeeInfoDataSeeder
    {
        private EmployeeInfoContext _EmployeeInfoContext;
        private const String EMPLOYEE_SEED_DATA_FILE = "resources/EmployeeSeedData.json";
        private const String COMPENSATION_SEED_DATA_FILE = "resources/CompensationSeedData.json";

        public EmployeeInfoDataSeeder(EmployeeInfoContext EmployeeInfoContext)
        {
            _EmployeeInfoContext = EmployeeInfoContext;
        }

/// <summary>
/// Seed the databse by reading from the associated JSON files.
/// The ReportingStructure entities are generated from the Employee seed data.
/// </summary>
        public async Task Seed()
        {
            if(!_EmployeeInfoContext.Employees.Any())
            {
                Console.WriteLine($"Loading employees from {EMPLOYEE_SEED_DATA_FILE}...");
                List<Employee> employees = LoadEmployees(EMPLOYEE_SEED_DATA_FILE);
                Console.WriteLine($"Saving employees...");
                _EmployeeInfoContext.Employees.AddRange(employees);

                await _EmployeeInfoContext.SaveChangesAsync();

                Console.WriteLine($"Employees saved");
            }
            if(!_EmployeeInfoContext.Compensations.Any())
            {
                Console.WriteLine($"Loading compensations from {COMPENSATION_SEED_DATA_FILE}...");
                List<Compensation> compensations = LoadCompensations(COMPENSATION_SEED_DATA_FILE, _EmployeeInfoContext.Employees.ToList());
                Console.WriteLine($"Saving compensations...");
                _EmployeeInfoContext.Compensations.AddRange(compensations);

                await _EmployeeInfoContext.SaveChangesAsync();
                Console.WriteLine($"Compensations saved");
            }
        }

/// <summary>
/// Reads from the Employee seed file and converts it to a list of Employees.
/// </summary>
        private List<Employee> LoadEmployees(string seedFile)
        {
            using (FileStream fs = new FileStream(seedFile, FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            using (JsonReader jr = new JsonTextReader(sr))
            {
                JsonSerializer serializer = new JsonSerializer();

                List<Employee> employees = serializer.Deserialize<List<Employee>>(jr);
                FixUpEmployeeReferences(employees);

                return employees;
            }
        }

/// <summary>
/// Reads from the Employee seed file and converts it to a list of Compensations.
/// </summary>
        private List<Compensation> LoadCompensations(string seedFile, List<Employee> employees)
        {
            using (FileStream fs = new FileStream(seedFile, FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            using (JsonReader jr = new JsonTextReader(sr))
            {
                JsonSerializer serializer = new JsonSerializer();

                List<Compensation> compensations = serializer.Deserialize<List<Compensation>>(jr);
                FixUpCompensationReferences(compensations, employees);

                return compensations;
            }
        }

        /// <summary>
        /// Fills in the referenced employees that are currently just an id.
        /// This matches the given id of a DirectReport, and replaces it with the full Employee entity.
        /// </summary>
        private void FixUpEmployeeReferences(List<Employee> employees)
        {
            var employeeIdRefMap = from employee in employees
                                select new { Id = employee.EmployeeId, EmployeeRef = employee };

            employees.ForEach(employee =>
            {
                
                if (employee.DirectReports != null)
                {
                    var referencedEmployees = new List<Employee>(employee.DirectReports.Count);
                    employee.DirectReports.ForEach(report =>
                    {
                        var referencedEmployee = employeeIdRefMap.First(e => e.Id == report.EmployeeId).EmployeeRef;
                        referencedEmployees.Add(referencedEmployee);
                    });
                    employee.DirectReports = referencedEmployees;
                }
            });
        }

/// <summary>
/// Fills in the referenced employees that are currently just an id.
/// This matches on the employeeid of a compensation, and replaceds it with a full Employee entity.
/// </summary>
        private void FixUpCompensationReferences(List<Compensation> compensations, List<Employee> employees)
        {
            foreach(Compensation compensation in compensations)
            {
                compensation.Employee = employees.First(e => e.EmployeeId == compensation.Employee.EmployeeId);
            }
        }
    }
}
