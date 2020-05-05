using challenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Services
{
    public interface IEmployeeService
    {
        Employee GetById(String id);
        ReportingStructure GetReportingStructureById(String id);
        Compensation GetCompensationById(String id);
        Compensation CreateCompensation(Compensation compensation);
        Employee Create(Employee employee);
        Employee Replace(Employee originalEmployee, Employee newEmployee);
    }
}
