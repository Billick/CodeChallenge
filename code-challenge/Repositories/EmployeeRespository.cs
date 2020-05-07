using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            //Employee employee = _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
            List<Employee> list = _employeeContext.Employees.ToList();
            Employee employee = list.SingleOrDefault(e => e.EmployeeId == id);

            return employee;
        }

        public Compensation AddCompensation(Compensation compensation)
        {
            compensation.Employee = GetById(compensation.EmployeeId);
            _employeeContext.Compensations.Add(compensation);
            return compensation;
        }

        public Compensation GetCompensationById(string id)
        {
            Compensation compensation = _employeeContext.Compensations.SingleOrDefault(c => c.Employee.EmployeeId == id);
            compensation.Employee = GetById(id);
            return compensation;
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }
    }
}
