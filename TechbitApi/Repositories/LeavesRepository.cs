using Dapper;
using System;
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
    public class LeavesRepository: ILeaveRepository
    {
        public IDbConnection Connection => new SqlConnection(Common.SqlConnectionString);

        public async Task<string> PostLeave(Leave leaveRequest) {
            using (IDbConnection conn = Connection){
                var query = "INSERT INTO UserLeave (EmployeeId,ReasonForLeave,LeaveType,StartLeave,EndLeave,ApproverId) Values (@EmployeeId,@ReasonForLeave,@LeaveType,@StartLeave,@EndLeave,@ApproverId);";
                try
                {
                    await conn.QueryAsync(query, new { EmployeeId = leaveRequest.EmployeeId, ReasonForLeave= leaveRequest.ReasonForLeave, LeaveType = leaveRequest.LeaveType, StartLeave = leaveRequest.StartLeave, EndLeave= leaveRequest.EndLeave, ApproverId= leaveRequest.ApproverId });
                }
                catch (Exception ex) {
                    throw ex;
                }
                
                return "leave applied successfully";
            }
        }

        public async Task<Leave> GetUserLeaves(string employeeId) {
            using (IDbConnection conn = Connection)
            {
                var qry = "select (select count(*) from UserLeave where EmployeeId = @EmployeeId) as NumberOfLeaves,(15-(select count(*) from UserLeave where EmployeeId=@EmployeeId)) as RemainingLeaves,u.IsApproved,e.Name as ApproverName,p.Name as EmployeeName,u.StartLeave,u.EndLeave,u.LeaveType  from UserLeave u join Employee e on u.ApproverId = e.EmployeeId left join Employee p on p.EmployeeId = u.EmployeeId  GROUP BY e.EmployeeId,e.Name,p.Name,u.IsApproved,u.StartLeave,u.EndLeave,u.LeaveType HAVING e.EmployeeId = @EmployeeId";
                var a = conn.QueryFirst<Leave>(qry, new { EmployeeId = employeeId });
                return a;
            }
        }

        public async Task<List<Leave>> GetLeaveApprovalRequest(string employeeId) {
            using (IDbConnection conn = Connection) {
                var qry = "select u.*,e.Name as ApproverName,p.Name as EmployeeName from UserLeave u left join Employee e on e.EmployeeId = u.ApproverId left join Employee p on p.EmployeeId = u.EmployeeId  where u.ApproverId = @EmployeeId AND u.IsApproved IS NULL";
                List<Leave> leaves = (await conn.QueryAsync<Leave>(qry, new { EmployeeId = employeeId })).ToList();
                return leaves;
            }
        }
        public async Task<string> SetApprove(Boolean isApprove, int leaveId) {
            using (IDbConnection conn = Connection)
            {
                var qry = "UPDATE UserLeave SET IsApproved = @IsApproved WHERE LeaveId = @LeaveId";
                await conn.QueryAsync(qry, new { IsApproved = isApprove, LeaveId = leaveId });
                return "approved successfully";
            }
        } 
    }
}
