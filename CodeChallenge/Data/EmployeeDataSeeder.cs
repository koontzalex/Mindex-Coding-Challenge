﻿using CodeChallenge.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Data
{
    public class EmployeeDataSeeder
    {
        private EmployeeInfoContext _EmployeeInfoContext;
        private const String EMPLOYEE_SEED_DATA_FILE = "resources/EmployeeSeedData.json";

        public EmployeeDataSeeder(EmployeeInfoContext EmployeeInfoContext)
        {
            _EmployeeInfoContext = EmployeeInfoContext;
        }

        public async Task Seed()
        {
            if(!_EmployeeInfoContext.Employees.Any())
            {
                List<Employee> employees = LoadEmployees();
                _EmployeeInfoContext.Employees.AddRange(employees);

                await _EmployeeInfoContext.SaveChangesAsync();
            }
        }

        private List<Employee> LoadEmployees()
        {
            using (FileStream fs = new FileStream(EMPLOYEE_SEED_DATA_FILE, FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            using (JsonReader jr = new JsonTextReader(sr))
            {
                JsonSerializer serializer = new JsonSerializer();

                List<Employee> employees = serializer.Deserialize<List<Employee>>(jr);
                FixUpReferences(employees);

                return employees;
            }
        }

        private void FixUpReferences(List<Employee> employees)
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
    }
}
