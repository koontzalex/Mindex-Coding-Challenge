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

/// <summary>
/// Saves a Compensation entity to the database.
/// The Compensation must reference a valid Employee.
/// The Compensation must not reference an Employee with an existing Compensation.
/// </summary>
/// <exception cref="NotSupportedException"></exception>
        public Compensation Create(Compensation compensation)
        {
            if(compensation != null)
            {
                Employee employee = _EmployeeRepository.GetById(compensation.Employee?.EmployeeId);
                if(employee == null)
                {
                    throw new NotSupportedException("Compensation references an invalid Employee");
                }
                if(_CompensationRepository.GetByEmployeeId(employee.EmployeeId) != null)
                {
                    throw new NotSupportedException("Compensation already exists for Employee");
                }

                compensation.Employee = employee;

                _CompensationRepository.Add(compensation);
                _CompensationRepository.SaveAsync().Wait();
            }

            return compensation;
        }

/// <summary>
/// Returns a Compensation entity based on the id, if one exists.
/// </summary>
        public Compensation GetById(string id)
        {
            if(String.IsNullOrEmpty(id))
            {
                return null;
            }
            
            Compensation compensation = _CompensationRepository.GetById(id);
            return compensation;
        }

/// <summary>
/// Updates the originalCompensation with the details of newCompensation.
/// It does not change the id.
/// The new compensation must reference a valid and unique employee.
/// </summary>
/// <exception cref="NotSupportedException"></exception>
        public Compensation Replace(Compensation originalCompensation, Compensation newCompensation)
        {
            if(originalCompensation != null)
            {
                Employee employee = _EmployeeRepository.GetById(newCompensation.Employee?.EmployeeId);
                if(employee == null)
                {
                    throw new NotSupportedException("Compensation references an invalid Employee");
                }
                _CompensationRepository.Remove(originalCompensation);
                if (newCompensation != null)
                {
                    newCompensation.Employee = employee;
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
