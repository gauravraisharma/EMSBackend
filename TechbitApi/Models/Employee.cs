using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechbitApi.Models
{
    public class Employee
    {
        public string EmployeeId { get; set; }

        public string Name { get; set; }

        public string EmpCode { get; set; }

        public int DesignationId { get; set; }

        public string DepartmentName { get; set; }

        public DateTime JoiningDate { get; set; }

        public string ContactNumber1 { get; set; }

        public string OfficialEmailId { get; set; }

        public string BloodGroup { get; set; }

        public int RoleId { get; set; }
    }
}
