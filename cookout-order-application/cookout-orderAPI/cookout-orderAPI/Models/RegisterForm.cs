using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cookout_orderAPI.Models
{
    public class RegisterForm
    {
        [Required]
        [EmailAddress(ErrorMessage = "A valid email address is required")]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
