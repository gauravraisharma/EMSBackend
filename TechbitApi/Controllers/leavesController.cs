using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechbitApi.Models;
using TechbitApi.Repositories.interfaces;

namespace TechbitApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class leavesController : ControllerBase
    {
        private ILeaveRepository _leaveRepository;
        private readonly IHttpContextAccessor _httpContext;

        public leavesController(ILeaveRepository leaveRepository, IHttpContextAccessor httpContext) {
            _leaveRepository = leaveRepository;
            _httpContext = httpContext;
        }
        [HttpGet]
        [Route("GetUserLeaves")]
        public async Task<ActionResult> GetUserLeaves() {
            string userId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            Leave leaves = await _leaveRepository.GetUserLeaves(userId);
            return Ok(leaves);
        }

        [HttpGet]
        [Route("GetLeaveApprovalRequests")]
        public async Task<ActionResult> GetLeaveApprovalRequests() {
            string userId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Leave> leaveRequests = await _leaveRepository.GetLeaveApprovalRequest(userId);
            return Ok(leaveRequests);

        }

        [HttpPost]
        [Route("Apply")]
        public async Task<ActionResult> Apply([FromBody] Leave leaveRequest) {
            leaveRequest.EmployeeId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var message = await _leaveRepository.PostLeave(leaveRequest);
            return Ok(message);
        }

        [HttpPatch]
        [Route("Approve")]
        public async Task<ActionResult> Approve([FromBody] ApproveRequest approvePayload) {
            var message = await _leaveRepository.SetApprove(approvePayload.IsApprove, approvePayload.LeaveId);
            return Ok(message);
        }
    }
}