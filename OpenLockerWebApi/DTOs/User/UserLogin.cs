using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLockerWebApi.DTOs.User
{
    /// <summary>
    /// DTO for User Login API
    /// </summary>
    public class UserLogin
    {

        /// <summary>
        /// Username of a user
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Password for the Username
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
