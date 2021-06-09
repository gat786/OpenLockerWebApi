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
using OpenLockerWebApi.DTOs;

namespace OpenLockerWebApi.Controllers
{
    /// <summary>
    /// Api Controller for all the User related fields such as register, login
    /// </summary>
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Default Constructor which is designed to work with DI of this project
        /// </summary>
        /// <param name="_userService">Implementation of the IUserService received from the DI container</param>
        /// <param name="mapper">Implementation of Automapper also received from the DI container</param>
        public UserController(IUserService _userService, IMapper mapper)
        {
            this._userService = _userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Controller to Enable Registration of users
        /// </summary>
        /// <param name="userCreateBody">Necessary Data for registering a user</param>
        /// <returns>CreatedResult if a success UnprocessableEntityResult if not successful</returns>
        [Route("register")]
        [HttpPost]
        public ActionResult<UserRead> Register(UserCreate userCreateBody)
        {
            var user = _mapper.Map<User>(userCreateBody);
            var userFound = _userService.GetByEmailOrUsername(emailAddress: user.EmailAddress, username: user.Username);
            // return a 422 if username or email address is already taken
            if (userFound != null)
            {
                var failedResponse = new StandardResponse { Success = false, Message = "Username or Email Address is already taken" };
                return new UnprocessableEntityObjectResult(failedResponse);
            };
            string passwordHash = HashGenerator.GenerateHash(user);
            user.Password = passwordHash;
            User insertedUser = _userService.CreateUser(user);
            return new CreatedResult(insertedUser.id, _mapper.Map<UserRead>(user));
        }

        /// <summary>
        /// Route to Facilitate User Login and generate JWT token as well as refresh token for authorised users
        /// </summary>
        /// <param name="loginCredentials">Username and Password for the user</param>
        /// <returns>Ok if it was successfull or ForbiddenResult if failed</returns>
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

                var SuccessFulResponse = new StandardResponse
                {
                    Success = true,
                    Message = "Successfully Logged In",
                    Data = userWithToken
                };
                return new OkObjectResult(SuccessFulResponse);
            }
            else
            {
                return new ForbidResult();
            }
        }

        /// <summary>
        /// Get a list of all registered Users
        /// </summary>
        /// <returns>Readable list of all registered users</returns>
        [Route("")]
        [HttpGet]
        public ActionResult<IEnumerable<UserRead>> GetUsers()
        {
            var usersList = _userService.GetAllUsers();
            var SuccessfulResponse = new StandardResponse
            {
                Success = true,
                Data = _mapper.Map<IEnumerable<UserRead>>(usersList)
            };
            return new OkObjectResult(SuccessfulResponse);
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
