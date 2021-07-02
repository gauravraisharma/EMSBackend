using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechbitApi.Models;
using TechbitApi.Repositories;
using TechbitApi.Repositories.interfaces;

namespace TechbitApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        [Route("GetEmployees")]
        public async Task<ActionResult> GetAllEmployees(int limit,int offset) {
            List<Employee> empList = await _employeeRepository.GetEmployees(limit,offset);
            return Ok(empList);
        }

        [HttpGet]
        [Route("GetEmployeeById/{employeeId}")]
        public async Task<ActionResult> GetEmployeeById(string employeeId) {
            EmployeeDetail empDetail = await _employeeRepository.GetEmployee(employeeId);
            return Ok(empDetail);
        }

        [HttpGet]
        [Route("Approvers")]
        public async Task<ActionResult> GetApprover() {
            List<Employee> assignees = await _employeeRepository.GetApproves();
            return Ok(assignees);
        }
    }
}
