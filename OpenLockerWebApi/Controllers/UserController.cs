using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using OpenLockerWebApi.DTOs.User;
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
        private readonly IMapper _mapper;

        public UserController(IUserService service,IMapper mapper)
        {
            _userService = service;
            _mapper = mapper;
        }



        [Route("register")]
        [HttpPost]
        public ActionResult<UserRead> Register(UserCreate userCreateBody)
        {
            var user = _mapper.Map<User>(userCreateBody);
            User insertedUser = _userService.CreateUser(user);
            return new CreatedResult(insertedUser.id,_mapper.Map<UserRead>(user));
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
