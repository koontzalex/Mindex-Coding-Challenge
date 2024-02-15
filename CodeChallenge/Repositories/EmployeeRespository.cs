using System;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public interface IEmployeeRepository
    {
        Employee GetById(String id);
        Employee Add(Employee employee);
        Employee Remove(Employee employee);
        Task SaveAsync();
    }
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeInfoContext _EmployeeInfoContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeInfoContext EmployeeInfoContext)
        {
            _EmployeeInfoContext = EmployeeInfoContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _EmployeeInfoContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            return _EmployeeInfoContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
        }

        public Task SaveAsync()
        {
            return _EmployeeInfoContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _EmployeeInfoContext.Remove(employee).Entity;
        }
    }
}
