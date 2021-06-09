using AutoMapper;
using Azure.Core;
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
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;

namespace OpenLockerWebApi.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController: ControllerBase
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
            var userFound = _userService.GetByEmailOrUsername(emailAddress: user.EmailAddress, username: user.Username);
            // return a 422 if username or email address is already taken
            if (userFound != null) return new UnprocessableEntityResult();
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
            // return 403 if user doesn't exist
            if (user == null) return new ForbidResult();
            var result = HashGenerator.ValidateHash(user, loginCredentials.Password);
            // return 403 if Password is not validated or else return Ok
            if (result)
            {
                var userWithToken = _mapper.Map<UserRead>(user);

                var jwtToken = JwtHelper.GenerateJwtToken(user);

                var ipAddress = GetIpAddress();
                var refreshToken = JwtHelper.GenerateRefreshToken(ipAddress);

                userWithToken.AccessToken = jwtToken;
                userWithToken.RefreshToken = refreshToken;
                return new OkObjectResult(userWithToken);
            }
            else
            {
                return new ForbidResult();
            }
        }

        [Route("")]
        [HttpGet]
        public ActionResult<IEnumerable<UserRead>> GetUsers()
        {
            var usersList = _userService.GetAllUsers();
            return new OkObjectResult(_mapper.Map<IEnumerable<UserRead>>(usersList));
        }

        private string GetIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
