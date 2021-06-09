using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OpenLockerWebApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenLockerWebApi.DTOs.User
{
    public class UserRead
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        public RefreshToken RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
