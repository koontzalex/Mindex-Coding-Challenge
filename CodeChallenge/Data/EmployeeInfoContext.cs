using System;
using CodeChallenge.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.Data
{
    public class EmployeeInfoContext : DbContext
    {
        public const String EMPLOYEE_INFO_DB_NAME = "EmployeeInfoDB";
        public EmployeeInfoContext(DbContextOptions<EmployeeInfoContext> options) : base(options)
        {
            
        }

/// <summary>
/// Add lazy loading for Employee.DirectReports.
/// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseLazyLoadingProxies();

        public DbSet<Employee> Employees { get; set; }
        public DbSet<ReportingStructure> ReportingStructures { get; set; }
        public DbSet<Compensation> Compensations { get; set; }
    }
}
