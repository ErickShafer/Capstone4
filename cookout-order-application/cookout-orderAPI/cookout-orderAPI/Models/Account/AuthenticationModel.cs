using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cookout_orderAPI.Models.Account
{
    /// <summary>
    /// Represents an authentication model request.
    /// </summary>
    public class AuthenticationModel
    {
        

        /// <summary>
        /// The user's password.
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// The user's email
        /// </summary>
        [Required]
        public string Email { get; set; }
        
    }
}
