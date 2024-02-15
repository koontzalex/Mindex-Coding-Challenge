using System;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public interface ICompensationService
    {
        Compensation GetById(String id);
        Compensation Create(Compensation Compensation);
        Compensation Replace(Compensation originalCompensation, Compensation newCompensation);
    }
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _CompensationRepository;
        private readonly IEmployeeRepository _EmployeeRepository;
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository CompensationRepository, IEmployeeRepository EmployeeRepository)
        {
            _CompensationRepository = CompensationRepository;
            _EmployeeRepository = EmployeeRepository;
            _logger = logger;
        }

        public Compensation Create(Compensation compensation)
        {
            if(compensation != null)
            {
                Employee employee = _EmployeeRepository.GetById(compensation.Employee);
                if(employee == null)
                {
                    throw new NotSupportedException("Compensation references an invalid Employee");
                }

                _CompensationRepository.Add(compensation);
                _CompensationRepository.SaveAsync().Wait();
            }

            return compensation;
        }

        public Compensation GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _CompensationRepository.GetById(id);
            }

            return null;
        }

        public Compensation Replace(Compensation originalCompensation, Compensation newCompensation)
        {
            if(originalCompensation != null)
            {
                Employee employee = _EmployeeRepository.GetById(newCompensation.Employee);
                if(employee == null)
                {
                    throw new NotSupportedException("Compensation references an invalid Employee");
                }
                _CompensationRepository.Remove(originalCompensation);
                if (newCompensation != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _CompensationRepository.SaveAsync().Wait();

                    _CompensationRepository.Add(newCompensation);
                    // overwrite the new id with previous Compensation id
                    newCompensation.CompensationId = originalCompensation.CompensationId;
                }
                _CompensationRepository.SaveAsync().Wait();
            }

            return newCompensation;
        }
    }
}
