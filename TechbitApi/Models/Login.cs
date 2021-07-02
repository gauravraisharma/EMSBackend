using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechbitApi.Models
{
    public class Login
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
