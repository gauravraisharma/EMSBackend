using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TechbitApi.Entities;
using TechbitApi.Models;
using TechbitApi.Repositories;
using TechbitApi.Repositories.interfaces;
using TechbitApi.Services;

namespace TechbitApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IEmployeeRepository _employeeRepository;
        private IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContext;
        public AccountController(IEmployeeRepository employeeRepository, IEmailSender emailSender, IHttpContextAccessor httpContext) {
            _employeeRepository = employeeRepository;
            _emailSender = emailSender;
            _httpContext = httpContext;
        }

        [Route("Auth")]
        [HttpPost]
        public async Task<IActionResult> Auth([FromBody] Login loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(ResponseStatusCode.BadRequest, ModelState.SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage).ToString()));

            var user = await _employeeRepository.Login(loginModel.UserName, loginModel.Password);
            var userRole = await _employeeRepository.GetEmployeeRole(user.RoleId);

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKey010203"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                claims: new List<Claim> {
                new Claim(ClaimTypes.Name, user.OfficialEmailId),
                new Claim(ClaimTypes.NameIdentifier, user.EmployeeId),
                new Claim(ClaimTypes.Role, userRole)
                },
                expires: DateTime.Now.AddDays(2),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new { Token = tokenString });
        }

        [Route("Me")]
        [HttpGet]
        public async Task<IActionResult> Me() {
            EmployeeDetail user = new EmployeeDetail();
            try {
                string userId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                user = await _employeeRepository.GetEmployee(userId);
            } catch (Exception e) {
                throw e;
            }
            return Ok(user);
        }

        [Route("ForgotPassword")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _employeeRepository.FindByEmail(model.email);
                await _emailSender.SendEmailAsync(model.email, "Reset Password",
                   $"password <a>link</a>");
            }
            return Ok();
        }
    }
}
