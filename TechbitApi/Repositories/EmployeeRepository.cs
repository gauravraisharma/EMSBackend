using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TechbitApi.Entities;
using TechbitApi.Models;
using TechbitApi.Repositories.interfaces;

namespace TechbitApi.Repositories
{
    public class EmployeeRepository: IEmployeeRepository
    {
        public IDbConnection Connection => new SqlConnection(Common.SqlConnectionString);

        public async Task<Employee> Login(string username, string password)
        {
            using (IDbConnection conn = Connection)
            {
                var qry = "SELECT * FROM Employee WHERE OfficialEmailId = @usr AND Password = @pwd";
                return conn.QueryFirstOrDefault<Employee>(qry, new { usr = username, pwd = password });
            }
        }
        public async Task<string> GetEmployeeRole(int roleId)
        {
            using (IDbConnection conn = Connection)
            {
                var qry = "SELECT Name FROM EmployeeRoles WHERE RoleId = @rid";
                return conn.QueryFirstOrDefault<string>(qry, new { rid = roleId });
            }
        }

        public async Task<List<Employee>> GetEmployees(int limit, int offset)
        {
            using (IDbConnection conn = Connection)
            {
                var qry = ";WITH x AS (SELECT EmployeeId FROM dbo.Employee ORDER BY EmployeeId OFFSET  @Limit * (@offset - 1) ROWS FETCH NEXT 10 ROWS ONLY) SELECT e.*,d.Name as DepartmentName FROM x INNER JOIN dbo.Employee AS e ON x.EmployeeId = e.EmployeeId left join Department as d On d.DepartmentId = e.DepartmentId;";
                List<Employee> empList = (await conn.QueryAsync<Employee>(qry, new { Limit = limit, Offset = offset })).ToList();
                return empList;
            }
        }

        public async Task<EmployeeDetail> GetEmployee(string employeeId)
        {
            using (IDbConnection conn = Connection)
            {
                var query = "select e.*,d.Name as DepartmentName from Employee e Inner join Department d On e.DepartmentId = d.DepartmentId where e.EmployeeId = @EmployeeId";
                EmployeeDetail empDetail = (await conn.QueryAsync<EmployeeDetail>(query, new { EmployeeId = employeeId })).FirstOrDefault();
                return empDetail;
            }
        }
        public async Task<Employee> FindByEmail(string email)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.Query<Employee>("SELECT * FROM Employee WHERE OfficialEmailId = @email", new { email = email }).FirstOrDefault();
            }
        }

        public async Task<List<Employee>> GetApproves()
        {
            using (IDbConnection conn = Connection)
            {
                var qry = "SELECT * FROM Employee WHERE RoleId=1";
                List<Employee> empList = (await conn.QueryAsync<Employee>(qry)).ToList();
                return empList;
            }
        }
    }
}
