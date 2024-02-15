using System;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation GetById(String id);
        Compensation Add(Compensation compensation);
        Compensation Remove(Compensation compensation);
        Task SaveAsync();
    }
    public class CompensationRepository : ICompensationRepository
    {
        private readonly EmployeeInfoContext _EmployeeInfoContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, EmployeeInfoContext EmployeeInfoContext)
        {
            _EmployeeInfoContext = EmployeeInfoContext;
            _logger = logger;
        }

        public Compensation Add(Compensation compensation)
        {
            compensation.CompensationId = Guid.NewGuid().ToString();
            if(_EmployeeInfoContext.Compensations.FirstOrDefault(savedCompensation => savedCompensation.Employee == compensation.Employee) != null)
            {
                throw new NotSupportedException($"Compensation for employee {compensation.Employee} already exists");
            }
            _EmployeeInfoContext.Compensations.Add(compensation);
            return compensation;
        }

        public Compensation GetById(string id)
        {
            return _EmployeeInfoContext.Compensations.SingleOrDefault(e => e.CompensationId == id);
        }

        public Compensation Remove(Compensation compensation)
        {
            return _EmployeeInfoContext.Remove(compensation).Entity;
        }

        public Task SaveAsync()
        {
            return _EmployeeInfoContext.SaveChangesAsync();
        }
    }
}
