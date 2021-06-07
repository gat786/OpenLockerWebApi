using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using OpenLockerWebApi.Models;
using OpenLockerWebApi.Services;
using OpenLockerWebApi.Services.UserService;
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
        private readonly IUserService _userService;
        public UserController(IUserService service)
        {
            _userService = service;
        }



        [Route("register")]
        [HttpPost]
        public string Register(User user)
        {
            Debug.WriteLine(user);
            _userService.CreateUser(user);
            return "Thanks mate";
        }

        [Route("login")]
        [HttpPost]
        public string Login(User user)
        {
            return "Logging in";
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return _userService.GetAllUsers();
        }
    }
}
