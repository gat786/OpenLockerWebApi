using AutoMapper;
using dotenv.net;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using OpenLockerWebApi.DTOs.User;
using OpenLockerWebApi.Helper;
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
            string passwordHash = HashGenerator.GenerateHash(user);
            user.Password = passwordHash;
            User insertedUser = _userService.CreateUser(user);
            return new CreatedResult(insertedUser.id,_mapper.Map<UserRead>(user));
        }

        [Route("login")]
        [HttpPost]
        public ActionResult Login(UserLogin loginCredentials)
        {
            User user = _userService.GetUserByUsername(loginCredentials.Username);
            if (user == null) return new ForbidResult();
            var result = HashGenerator.ValidateHash(user, loginCredentials.Password);
            return result ? new OkResult() : new ForbidResult();
        }

        [Route("")]
        [HttpGet]
        public ActionResult<IEnumerable<UserRead>> GetUsers()
        {
            var usersList = _userService.GetAllUsers();
            return new OkObjectResult(_mapper.Map<IEnumerable<UserRead>>(usersList));
        }
    }
}
