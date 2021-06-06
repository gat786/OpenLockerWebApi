using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using OpenLockerWebApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLockerWebApi.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController
    {

        [Route("register")]
        [HttpPost]
        public string Register(User user)
        {
            Debug.WriteLine(user);
            return "Thanks mate";
        }
        [Route("login")]
        [HttpPost]
        public string Login(User user)
        {
            return "Logging in";
        }
    }
}
