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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
        /// <remarks>
        /// How to Call:
        ///     POST /user/register
        /// 
        /// Sample Request:
        ///     {
        ///         "UserName": "GoodUserName",
        ///         "EmailAddress": "Email@server.com",
        ///         "Password": "SomeVeryLongPassword428@"
        ///     }
        /// </remarks>
        /// <param name="userCreateBody">Necessary Data for registering a user</param>
        /// <returns>CreatedResult if a success UnprocessableEntityResult if not successful</returns>
        [Route("register")]
        [HttpPost]
        public ActionResult<UserRead> Register(UserCreate userCreateBody)
        {
            var user = _mapper.Map<User>(userCreateBody);
            var userFound = _userService.GetUserByEmail(emailAddress: userCreateBody.EmailAddress);
            //var userFound = _userService.GetByEmailOrUsername(emailAddress: user.EmailAddress, username: user.Username);
            // return a 422 if username or email address is already taken
            if (userFound != null)
            {
                var failedResponse = new StandardResponse { Success = false, Message = "Email Address is already taken" };
                return new UnprocessableEntityObjectResult(failedResponse);
            };

            userFound = _userService.GetUserByUsername(userName: userCreateBody.Username);

            if (userFound != null)
            {
                var failedResponse = new StandardResponse { Success = false, Message = "Username is already taken" };
                return new UnprocessableEntityObjectResult(failedResponse);
            };

            string passwordHash = HashGenerator.GenerateHash(user);

            var jwtToken = JwtHelper.GenerateJwtToken(user);

            var ipAddress = GetIpAddress();
            var refreshToken = JwtHelper.GenerateRefreshToken(ipAddress);

            var userToWrite = new User
            {
                Username = user.Username,
                EmailAddress = user.EmailAddress,
                Password = passwordHash,
                RefreshToken = refreshToken
            };

            setTokenCookie(refreshToken.Token);

            var insertedUser = _userService.CreateUser(userToWrite);

            var response = new StandardResponse
            {
                Success = true,
                Message = "User created successfully",
                Data = new UserRead
                {
                    Username = user.Username,
                    EmailAddress = user.EmailAddress,
                    AccessToken = jwtToken,
                    RefreshToken = refreshToken
                }
            };
            return new CreatedResult(insertedUser.id, response);
        }

        /// <summary>
        /// Route to Facilitate User Login and generate JWT token as well as refresh token for authorised users
        /// </summary>
        /// <remarks>
        /// How to Call:
        ///     POST /user/login
        /// Sample Request:
        ///     {
        ///         "Username": "gat786"
        ///         "Password": "SomeSecretPassword3423@!"
        ///     }
        /// </remarks>
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
                var jwtToken = JwtHelper.GenerateJwtToken(user);
                var userWithToken = new UserRead
                {
                    Username = user.Username,
                    EmailAddress = user.EmailAddress,
                };

                userWithToken.AccessToken = jwtToken;
                if(user.RefreshToken != null && user.RefreshToken.IsExpired)
                {
                    var ipAddress = GetIpAddress();
                    var newRefreshToken = JwtHelper.GenerateRefreshToken(ipAddress);
                    string newJwtToken = JwtHelper.GenerateJwtToken(user);
                    UserRead userRead = new UserRead
                    {
                        Username = user.Username,
                        EmailAddress = user.EmailAddress,
                        AccessToken = newJwtToken,
                        RefreshToken = newRefreshToken
                    };

                    StandardResponse response = new StandardResponse
                    {
                        Success = true,
                        Message = "User Logged In Successfully",
                        Data = userRead
                    };
                    return new OkObjectResult(response);
                }

                userWithToken.RefreshToken = user.RefreshToken;

                setTokenCookie(userWithToken.RefreshToken.Token);

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
        /// <remarks>
        /// How to Call:
        ///     GET /user/
        /// </remarks>
        /// <returns>Readable list of all registered users</returns>
        [Route("")]
        [Authorize]
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

        /// <summary>
        /// Get Access Token and also refresh token when you already have refresh token cookie
        /// </summary>
        /// <remarks>
        ///     Add refreshToken cookie to while making this request
        /// </remarks>
        /// <response code="200">Tokens are returned successfully</response>
        /// <response code="403">Refresh token has expired and you need to call the login api</response>
        /// <returns>
        /// Valid Access and refresh tokens
        /// </returns>
        [AllowAnonymous]
        [HttpPost("get-new-tokens")]
        public ActionResult GetAccessToken()
        {
            var ipAddress = GetIpAddress();
            var refreshToken = Request.Cookies["refreshToken"];
            var user = _userService.GetUserFromRefreshToken(refreshToken);

            if (user.RefreshToken.IsActive)
            {
                var jwtToken = JwtHelper.GenerateJwtToken(user);
                UserRead userRead = new UserRead
                {
                    Username = user.Username,
                    EmailAddress = user.EmailAddress,
                    AccessToken = jwtToken,
                    RefreshToken = user.RefreshToken
                };

                StandardResponse response = new StandardResponse
                {
                    Success = true,
                    Message = "User Logged In Successfully",
                    Data = userRead
                };
                return new OkObjectResult(response);
            }
            else
            {
                return new BadRequestObjectResult(new StandardResponse{ Success = false, Message = "Refresh token has expired you need to relogin" });
            }
        }


        private string GetIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
