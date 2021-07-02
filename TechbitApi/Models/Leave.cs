using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechbitApi.Models
{
    public class Leave
    {
        public int? LeaveId { get; set; }
        public string? EmployeeId { get; set; }

        public string? EmployeeName { get; set; }

        public string ReasonForLeave { get; set; }

        public int LeaveType { get; set; } 

        public string LeaveApproval { get; set; }

        public DateTime StartLeave { get; set; }

        public DateTime EndLeave { get; set; }

        public bool? IsApproved { get; set; }

        public string ApproverId { get; set; }

        public int? NumberOfLeaves { get; set; }

        public int? RemainingLeaves { get; set; }

        public string? ApproverName { get; set; }
    }
}
