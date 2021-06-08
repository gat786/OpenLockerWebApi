using Microsoft.AspNetCore.Identity;
using OpenLockerWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLockerWebApi.Helper
{
    public static class HashGenerator
    {
        /// <summary>
        /// Generate Hash for the Password of Provided user
        /// </summary>
        /// <param name="user">The user for which password is to be generated</param>
        /// <returns>Hashed Password string</returns>
        public static string GenerateHash(User user)
        {
            var hasher = new PasswordHasher<User>();
            string hashedPassword = hasher.HashPassword(user, user.Password);
            return hashedPassword;
        }

        public static bool ValidateHash(User user, string providedPassword)
        {
            var hasher = new PasswordHasher<User>();
            var hashresult = hasher.VerifyHashedPassword(user, user.Password, providedPassword);
            return hashresult == PasswordVerificationResult.Success;
        }
    }
}
