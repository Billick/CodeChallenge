﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Employee Create(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }

            return employee;
        }

        public Employee GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetById(id);
            }

            return null;
        }

        

        public ReportingStructure GetReportingStructureById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                ReportingStructure ret = new ReportingStructure();
                Employee e = _employeeRepository.GetById(id);
                ret.Employee = e;
                ret.NumberOfReports = GetReportCount(e.DirectReports);
                return ret;
            }
            return null;
        }

        public Compensation GetCompensationById(string id)
        {

            if (!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetCompensationById(id);
            }
            return null;
        }

        public Compensation CreateCompensation(Compensation compensation)
        {
            if(compensation != null)
            {
                _employeeRepository.AddCompensation(compensation);
                _employeeRepository.SaveAsync().Wait();
            }
            return compensation;
        }

        private int GetReportCount(List<Employee> reports)
        {
            if (reports == null || reports.Count == 0)
                return 0;
            int count = 0;
            foreach(Employee r in reports)
            {
                Employee e = _employeeRepository.GetById(r.EmployeeId);
                count += 1 + (e.DirectReports == null ? 0 : GetReportCount(e.DirectReports));
            }
            return count;
        }

        public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if(originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }
    }
}
