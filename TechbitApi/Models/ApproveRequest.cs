using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechbitApi.Models
{
    public class ApproveRequest
    {
        public bool IsApprove { get; set; }
        public int LeaveId { get; set; }
    }
}
