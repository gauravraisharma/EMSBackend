using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechbitApi.Models;

namespace TechbitApi.Repositories.interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee> Login(string username, string password);
        Task<string> GetEmployeeRole(int roleId);

        Task<List<Employee>> GetEmployees(int limit, int offset);

        Task<EmployeeDetail> GetEmployee(string employeeId);

        Task<Employee> FindByEmail(string email);

        Task<List<Employee>> GetApproves();
    }
}
