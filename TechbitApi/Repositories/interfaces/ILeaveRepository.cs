using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechbitApi.Models;

namespace TechbitApi.Repositories.interfaces
{
    public interface ILeaveRepository
    {
        Task<string> PostLeave(Leave leaveRequest);
        Task<Leave> GetUserLeaves(string employeeId);
        Task<List<Leave>> GetLeaveApprovalRequest(string employeeId);
        Task<string> SetApprove(Boolean isApprove, int leaveId);
    }
}
